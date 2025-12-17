using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.DTO
{
    public class MessageResultDto
    {
        public Guid MessageId { get; set; }
        public Guid ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; }
        public bool IsDeleted { get; set; }
        public string MessageContent { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
}
