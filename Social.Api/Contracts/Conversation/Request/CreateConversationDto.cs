using System.ComponentModel.DataAnnotations;

namespace Social.Api.Contracts.Conversation.Request
{
    public class CreateConversationDto
    {
        public string? Name { get; set; }
        public string? PhotoUrl { get; set; }
        public List<string> ParticipantsIds { get; set; } = new List<string>();
    }
}
