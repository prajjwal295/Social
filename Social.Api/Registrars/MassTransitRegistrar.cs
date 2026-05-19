
using MassTransit;
using Social.Infrastructure.Messaging.Consumer;
using Social.Infrastructure.Messaging.Consumers;

namespace Social.Api.Registrars
{
    public class MassTransitRegistrar : IWebApplicationBuilderRegistar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddMassTransit(config =>
            {
                //Add All the Consumers Here
                config.AddConsumer<UserRegisteredConsumer>(c =>
                {
                    c.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)
                    ));
                });

                config.AddConsumer<PostCreatedConsumer>(c =>
                {
                    c.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)
                    ));
                });

                config.AddConsumer<PostLikedConsumer>(c =>
                {
                    c.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)
                    ));
                });

                config.AddConsumer<UserCommentedConsumer>(c =>
                {
                    c.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)
                    ));
                });

                config.AddConsumer<UserFollowedConsumer>(c =>
                {
                    c.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15),
                        TimeSpan.FromSeconds(30)
                    ));
                });

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(builder.Configuration["RabbitMQ:Host"], h =>
                    {
                        h.Username(builder.Configuration["RabbitMQ:Username"]);
                        h.Password(builder.Configuration["RabbitMQ:Password"]);
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
    }
}
