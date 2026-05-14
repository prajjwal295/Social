namespace Social.Infrastructure.Messaging.Events
{
    public class UserRegisteredEvent
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
    }
}
