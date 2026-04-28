using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using Social.Application.Common.Events;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.CelebrityPostCacheAggregate;
using Social.Domain.Aggregates.UserFeedAggregate;
using Social.Infrastructure.Messaging.RabbitMQ;
using System.Text;

public class PostCreatedConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RabbitMqConnection _connection;

    public PostCreatedConsumer(
        IServiceScopeFactory scopeFactory,
        RabbitMqConnection connection)
    {
        _scopeFactory = scopeFactory;
        _connection = connection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var conn = await _connection.CreateConnectionAsync();
        var channel = await conn.CreateChannelAsync();

        var queueName = nameof(PostCreatedEvent);

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            try
            {
                var body = args.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var @event = JsonConvert.DeserializeObject<PostCreatedEvent>(json);

                if (@event == null)
                {
                    await channel.BasicAckAsync(args.DeliveryTag, false);
                    return;
                }

                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();

                var authorId = @event.UserId;

                // Check if the author is a celebrity (has more than 1000 followers)
                var user = await db.UserProfiles.FindAsync(authorId);
                bool isCelebrity = user.FollowersCount > 1000;

                if(isCelebrity)
                {
                    var celebrityPost = CelebrityPostCache.Create(@event.PostId,authorId, @event.CreatedAt);
                    await db.CelebrityPostCache.AddAsync(celebrityPost);
                }
                else
                {
                    var followerIds = await db.Followers
                                    .Where(f => f.FolloweeId == authorId && f.UnfollowedAt == null)
                                    .Select(f => f.FollowerId)
                                    .ToListAsync();

                    followerIds.Add(authorId);

                    foreach (var userId in followerIds)
                    {
                        var feed = await db.UserFeed
                            .FirstOrDefaultAsync(f => f.UserProfileId == userId);

                        if (feed == null)
                        {
                            feed = UserFeed.Create(userId);
                            await db.UserFeed.AddAsync(feed);
                        }

                        var alreadyExists = await db.FeedItems
                            .AnyAsync(x =>
                                x.UserFeedId == feed.UserFeedId &&
                                x.PostId == @event.PostId);

                        if (alreadyExists)
                            continue;

                        feed.AddPost(@event.PostId, @event.CreatedAt);
                    }
                }

                await db.SaveChangesAsync();

                await channel.BasicAckAsync(args.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await channel.BasicNackAsync(args.DeliveryTag, false, true);
            }
        };

        await channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumerTag: "",
            noLocal: false,
            exclusive: false,
            arguments: null,
            consumer: consumer
        );

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}