using System.ComponentModel.DataAnnotations;

namespace Social.Api.Contracts.Conversation.Request
{
    public class SendMessageDto
    {
        [Required]
        public string TextContent { get; set; }
    }
}
