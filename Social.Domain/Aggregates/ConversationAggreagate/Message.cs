using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Aggregates.ConversationAggreagate
{
    public class Message
    {
        private Message()
        {

        }

        public Guid MessageId { get; private set; }
        public Guid ConversationId { get; private set; }
        public Guid SenderId { get; private set; }
        public string MessageContent { get; private set; }
        
        //add media later and delete features

        public DateTime SentAt { get; private set; }
        public bool IsDeleted { get; private set; }


        public static Message CreateMessage(Guid conversationId, Guid senderId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Message content cannot be empty.", nameof(content));
            
            return new Message
            {
                ConversationId = conversationId,
                SenderId = senderId,
                MessageContent = content.Trim(),
                SentAt = DateTime.UtcNow,
                IsDeleted = false,
            };
        }

        public void EditContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("Message content cannot be empty.", nameof(newContent));

            MessageContent = newContent.Trim();
        }

        public  void Delete()
        {
            IsDeleted = true;
            MessageContent = "[deleted]";
        }

    }
}
