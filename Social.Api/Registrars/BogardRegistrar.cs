using Microsoft.Extensions.DependencyInjection;
using Social.Api.Mappings;
using Social.Application.UserProfiles.Queries;
using MediatR;
using Social.Application.Common;

namespace Social.Api.Registrars
{
    public class BogardRegistrar : IWebApplicationBuilderRegistar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowClient", policy =>
                {
                    //policy.WithOrigins(
                    //    "http://127.0.0.1:5500",
                    //    "http://localhost:5173",
                    //    "http://localhost:3001"
                    //    )

                     policy.SetIsOriginAllowed(_ => true)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
            builder.Services.AddSignalR();
            builder.Services.AddAutoMapper(typeof(ProfileMappings).Assembly);
            builder.Services.AddAutoMapper(typeof(PostMappings).Assembly);
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(GetAllUserProfiles).Assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBeahviour<,>));
                cfg.AddOpenBehavior(typeof(CachingBehaviour<,>));
            });
            builder.Services.AddDistributedMemoryCache();
        }
    }
}
