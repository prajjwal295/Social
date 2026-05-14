namespace Social.Infrastructure.Messaging.Events
{
    public class PostCreatedEvent
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
