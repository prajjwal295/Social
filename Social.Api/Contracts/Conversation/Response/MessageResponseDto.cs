namespace Social.Api.Contracts.Conversation.Response
{
    public class MessageResponseDto
    {
        public Guid MessageId { get;  set; }
        public Guid ConversationId { get;  set; }
        public Guid SenderId { get;  set; }
        public string MessageContent { get;  set; }
        public DateTime SentAt { get;  set; }
    }
}
