namespace Social.Api.Registrars
{
    public interface IWebApplicationRegistar
    {
        public void RegisterPipelineComponents(WebApplication app);
    }
}
