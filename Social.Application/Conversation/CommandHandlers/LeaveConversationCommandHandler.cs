using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Conversation.Commands;
using Social.Application.Models;
using Social.DAL.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Conversation.CommandHandlers
{
    internal class LeaveConversationCommandHandler : IRequestHandler<LeaveConversationCommand, OperationResult<Unit>>
    {
        private readonly DataContext _dbContext;

        public LeaveConversationCommandHandler(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<Unit>> Handle(LeaveConversationCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Unit>();
            try
            {
                var conversation = await _dbContext.Conversations
                    .FirstOrDefaultAsync(x => x.ConversationId == request.ConversationId && x.IsGroup == true);

                if(conversation == null)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error
                    {
                        Message = "No Conversation Found Related to that Information",
                        Code = Enums.ErrorCode.NotFound
                    });
                    return result;
                }

                var conversationParticipantExits = await _dbContext.ConversationParticipants
                    .FirstOrDefaultAsync(x => x.ConversationId == request.ConversationId && x.UserProfileId == request.UserProfileId);

                if(conversationParticipantExits == null)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error
                    {
                        Message = "Unauthorized",
                        Code = Enums.ErrorCode.Unauthorized
                    });
                    return result;
                }

                _dbContext.ConversationParticipants.Remove(conversationParticipantExits);
                await _dbContext.SaveChangesAsync();
                result.Payload = Unit.Value;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                var error = new Error
                {
                    Message = "Server Error",
                    Code = Enums.ErrorCode.ServerError
                };
                result.Errors.Add(error);
            }
            return result;
        }
    }
}
