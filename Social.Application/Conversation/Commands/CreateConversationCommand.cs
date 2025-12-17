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
    public class CreateConversationCommand : IRequest<OperationResult<Social.Domain.Aggregates.ConversationAggreagate.Conversation>>
    {
        public string? Name { get; set; }
        public string? PhotoUrl {  get; set; }
        public Guid CreatedBy { get; set; }
        public List<Guid> Participants { get; set; } = new List<Guid>();
    }
}
