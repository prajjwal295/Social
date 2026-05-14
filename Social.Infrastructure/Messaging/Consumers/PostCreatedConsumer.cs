using MassTransit;
using Microsoft.EntityFrameworkCore;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.CelebrityPostCacheAggregate;
using Social.Domain.Aggregates.UserFeedAggregate;
using Social.Infrastructure.Messaging.Events;

public class PostCreatedConsumer : IConsumer<PostCreatedEvent>
{
    private readonly DataContext _context;
    public PostCreatedConsumer(DataContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<PostCreatedEvent> context)
    {
        PostCreatedEvent @event = context.Message;

        try
        {
            if (@event == null)
            {
                return;
            }
            var authorId = @event.UserId;

            // Check if the author is a celebrity (has more than 1000 followers)
            var user = await _context.UserProfiles.FindAsync(authorId);
            bool isCelebrity = user.FollowersCount > 1000;

            if (isCelebrity)
            {
                var celebrityPost = CelebrityPostCache.Create(@event.PostId, authorId, @event.CreatedAt);
                await _context.CelebrityPostCache.AddAsync(celebrityPost);
            }
            else
            {
                var followerIds = await _context.Followers
                                .Where(f => f.FolloweeId == authorId && f.UnfollowedAt == null)
                                .Select(f => f.FollowerId)
                                .ToListAsync();

                followerIds.Add(authorId);

                foreach (var userId in followerIds)
                {
                    var feed = await _context.UserFeed
                        .FirstOrDefaultAsync(f => f.UserProfileId == userId);

                    if (feed == null)
                    {
                        feed = UserFeed.Create(userId);
                        await _context.UserFeed.AddAsync(feed);
                    }

                    var alreadyExists = await _context.FeedItems
                        .AnyAsync(x =>
                            x.UserFeedId == feed.UserFeedId &&
                            x.PostId == @event.PostId);

                    if (alreadyExists)
                        continue;

                    feed.AddPost(@event.PostId, @event.CreatedAt);
                }
            }

            await _context.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}