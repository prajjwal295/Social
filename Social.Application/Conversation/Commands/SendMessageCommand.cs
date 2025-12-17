using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.ConversationAggreagate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Conversation.Commands
{
    public class SendMessageCommand : IRequest<OperationResult<Message>>
    {
        public Guid SenderId { get; set; }
        public string Message { get; set; }
        public Guid ConversationId { get; set; }
    }
}
