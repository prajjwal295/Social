
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Social.DAL.DbContext;
using Social.DAL.DbContext.Interceptors;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Api.Registrars
{
    public class DbContextRegistrar : IWebApplicationBuilderRegistar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            var cs = builder.Configuration.GetConnectionString("SocialConnectionString");

            builder.Services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(cs);
            });

            builder.Services.AddIdentityCore<IdentityUser>()
                .AddEntityFrameworkStores<DataContext>();
        }
    }
}
