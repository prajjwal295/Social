using MediatR;
using Social.Application.DTO;
using Social.Application.Models;
using Social.Domain.Aggregates.ConversationAggreagate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Conversation.Query
{
    public class GetMessageQuery : IRequest<OperationResult<List<MessageResultDto>>>
    {
        public Guid UserProfileId { get; set; }
        public Guid ConversationId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
