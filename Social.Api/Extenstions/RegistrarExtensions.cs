using Social.Api.Registrars;

namespace Social.Api.Extensions
{
    public static class RegistrarExtensions
    {
        //Your extensions will then reflect over that assembly, find all classes implementing:

        //        IWebApplicationBuilderRegistrar → to configure services/DI

        //        IWebApplicationRegistrar → to configure middleware pipeline

        //So in Program.cs, you replace a bunch of boilerplate service registrations and middleware setup with just those two calls.
        public static void RegisterServices(this WebApplicationBuilder builder, Type scanningType)
        {
            var registrars = scanningType.Assembly.GetTypes()
                .Where(t => typeof(IWebApplicationBuilderRegistar).IsAssignableFrom(t)
                            && !t.IsInterface
                            && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IWebApplicationBuilderRegistar>();

            foreach (var registrar in registrars)
            {
                registrar.RegisterServices(builder);
            }
        }

        public static void RegisterPipelineComponents(this WebApplication app, Type scanningType)
        {
            var registrars = scanningType.Assembly.GetTypes()
                .Where(t => typeof(IWebApplicationRegistar).IsAssignableFrom(t)
                            && !t.IsInterface
                            && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IWebApplicationRegistar>();

            foreach (var registrar in registrars)
            {
                registrar.RegisterPipelineComponents(app);
            }
        }
    }
}
