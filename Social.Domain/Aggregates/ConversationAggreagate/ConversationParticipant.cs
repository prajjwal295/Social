using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Aggregates.ConversationAggreagate
{
    public class ConversationParticipant
    {
        private ConversationParticipant()
        {

        }

        public Guid ConversationParticipantId { get; private set; }
        public Guid ConversationId { get; private set; }
        public Guid UserProfileId { get; private set; }
        public DateTime JoinedAt { get; private set; }
        public UserRoles Role { get; private set; }


        public static  ConversationParticipant CreateConversationParticipant(Guid conversationId, Guid userProfileId, UserRoles role = UserRoles.Member)
        {
            return new ConversationParticipant
            {
                ConversationId = conversationId,
                UserProfileId = userProfileId,
                Role = role,
                JoinedAt = DateTime.UtcNow
            };
        }

        public void Promote(UserRoles newRole)
        {
            if (newRole < Role)
                throw new InvalidOperationException("Cannot demote via promote method.");

            Role = newRole;
        }

        public void Demote(UserRoles newRole)
        {
            if (newRole > Role)
                throw new InvalidOperationException("Cannot promote via demote method.");

            Role = newRole;
        }
    }
}
