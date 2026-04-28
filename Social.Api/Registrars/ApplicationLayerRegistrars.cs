using Social.Application.Common.Interface;
using Social.Application.Services;
using Social.Infrastructure.Messaging.RabbitMQ;

namespace Social.Api.Registrars
{
    public class ApplicationLayerRegistrars : IWebApplicationBuilderRegistar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddSingleton<RabbitMqConnection>();
            builder.Services.AddScoped<IEventBus, RabbitMqEventBus>();
            builder.Services.AddHostedService<PostCreatedConsumer>();
        }
    }
}
