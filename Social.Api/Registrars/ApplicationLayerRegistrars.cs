
using Social.Application.Services;

namespace Social.Api.Registrars
{
    public class ApplicationLayerRegistrars : IWebApplicationBuilderRegistar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<JwtService>();
        }
    }
}
