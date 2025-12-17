using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Social.Api.Filters;

namespace Social.Api.Registrars
{
    public class MvcRegistrar : IWebApplicationBuilderRegistar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(config =>
            {
                config.Filters.Add(typeof(SocialExceptionHandler));
            });

            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;

                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddSignalR();
        }
    }
}
