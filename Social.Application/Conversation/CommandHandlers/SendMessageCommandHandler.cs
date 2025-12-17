using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Social.Application.Conversation.Commands;
using Social.Application.Models;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.ConversationAggreagate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Conversation.CommandHandlers
{
    internal class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, OperationResult<Message>>
    {
        private readonly DataContext _context;

        public SendMessageCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Message>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Message>();
            try
            {
                bool checkParticipantExist = await _context.ConversationParticipants.AnyAsync(x => x.ConversationId == request.ConversationId && x.UserProfileId == request.SenderId);

                if(!checkParticipantExist)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = Enums.ErrorCode.Unauthorized,
                        Message = $"The Following User {request.SenderId} is not authorized to access this Conversation"
                    };
                    result.Errors.Add(error);
                    return result;
                }

                var message = Message.CreateMessage(request.ConversationId, request.SenderId, request.Message);

                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();

                result.Payload = message;
                return result;
            }
            catch (Exception ex)
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
