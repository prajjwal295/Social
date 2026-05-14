namespace Social.Api.Registrars
{
    public class MvcWebAppRegistrar : IWebApplicationRegistar
    {
        public void RegisterPipelineComponents(WebApplication app)
        {
            app.UseCors("AllowClient");
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();  
            app.UseAuthorization();

            app.MapControllers();
            // app.MapHub<ChatHub>("/chathub");
        }
    }
}
