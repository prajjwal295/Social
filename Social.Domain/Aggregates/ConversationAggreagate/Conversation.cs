using Microsoft.VisualBasic;
using Social.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Social.Domain.Aggregates.ConversationAggreagate
{
    public class Conversation
    {
        private Conversation()
        {

        }

        private readonly List<ConversationParticipant> _participants = new();
        private readonly List<Message> _messages = new();

        public Guid ConversationId { get; private set; }
        public bool IsGroup { get; private set; }
        public string? Name { get; private set; }
        public string? PhtotoUrl { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public Guid? CreatedBy { get; private set; }

        public IEnumerable<ConversationParticipant> Participant { get { return _participants; } }
        public IEnumerable<Message> Messages { get { return _messages; } }


        public static Conversation CreateOneToOneConversation(Guid userProfileId1, Guid UserProfileId2)
        {
            if (userProfileId1 == UserProfileId2)
                throw new ArgumentException("A user cannot start a 1:1 conversation with themselves.");

            var conversationId = Guid.NewGuid();
            var conversation = new Conversation
            {
                ConversationId = conversationId,
                IsGroup = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userProfileId1
            };

            var participant1 = ConversationParticipant.CreateConversationParticipant(conversationId, userProfileId1);
            var participant2 = ConversationParticipant.CreateConversationParticipant(conversationId, UserProfileId2);


            conversation._participants.Add(participant1);
            conversation._participants.Add(participant2);

            return conversation;
        }


        public static Conversation CreateConversation(string name, string? PhotoUrl, Guid userProfileId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Group conversation must have a name.", nameof(name));

            var conversationId = Guid.NewGuid();
            var conversation = new Conversation
            {
                ConversationId = conversationId,
                IsGroup = true,
                Name = name.Trim(),
                PhtotoUrl = PhotoUrl,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userProfileId
            };


            var creator = ConversationParticipant.CreateConversationParticipant(conversationId, userProfileId, UserRoles.Owner);
            conversation._participants.Add(creator);

            return conversation;
        }

        public void AddMessage(Message message)
        {
            _messages.Add(message);
        }

        public void AddParticipants(ConversationParticipant participant)
        {
            _participants.Add(participant);
        }

        public void RemoveParticipants(ConversationParticipant participant)
        {
            _participants.Remove(participant);
        }


    }
}
