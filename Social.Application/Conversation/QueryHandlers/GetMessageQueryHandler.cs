using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Conversation.Query;
using Social.Application.DTO;
using Social.Application.Models;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.ConversationAggreagate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Conversation.QueryHandlers
{
    internal class GetMessageQueryHandler : IRequestHandler<GetMessageQuery, OperationResult<List<MessageResultDto>>>
    {
        private readonly DataContext _context;

        public GetMessageQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<MessageResultDto>>> Handle(GetMessageQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<MessageResultDto>>();
            try
            {
                var validateUser = await _context.ConversationParticipants
                    .AnyAsync(u => u.ConversationId == request.ConversationId && u.UserProfileId == request.UserProfileId);

                if (!validateUser)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = Enums.ErrorCode.Unauthorized,
                        Message = $"The Following User {request.UserProfileId} is not authorized to access this Conversation"
                    };
                    result.Errors.Add(error);
                    return result;
                }

                var skipCount = (request.PageNumber - 1) * request.PageSize;

                var response= await (from m in _context.Messages
                 join u in _context.UserProfiles
                 on m.SenderId equals u.UserProfileId
                 where m.ConversationId == request.ConversationId
                 orderby m.SentAt descending
                 select new MessageResultDto
                 {
                     MessageId = m.MessageId,
                     ConversationId = m.ConversationId,
                     SenderId = m.SenderId,
                     MessageContent = m.MessageContent,
                     SenderName = u.BasicInfo.FirstName + u.BasicInfo.LastName,
                     SenderEmail = u.BasicInfo.EmailAddress,
                     SentAt = m.SentAt,
                     IsDeleted = m.IsDeleted
                 }).Skip(skipCount)
                    .Take(request.PageSize)
                    .ToListAsync();

                 result.Payload = response;
                return result;
            }
            catch(Exception ex)
            {
                result.IsError = true;
                var error = new Error
                {
                    Code = Enums.ErrorCode.UnknownError,
                    Message = ex.Message
                };
                result.Errors.Add(error);
            }
            return result;
        }
    }
}
