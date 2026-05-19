using Social.Application.Options;
using Social.Application.Services;
using Social.Infrastructure.Cloudinary;
using Social.Infrastructure.Email;

namespace Social.Api.Registrars
{
    public class ApplicationLayerRegistrars : IWebApplicationBuilderRegistar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.Configure<CloudinarySettings>(
            builder.Configuration.GetSection("CloudinarySettings"));

            builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
        }
    }
}
