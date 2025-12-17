using CwkSocial.Api.Options;
using Social.Api.Registrars;

namespace CwkSocial.Api.Registrars
{
    public class SwaggerRegistrar : IWebApplicationBuilderRegistar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
        }
    }
}