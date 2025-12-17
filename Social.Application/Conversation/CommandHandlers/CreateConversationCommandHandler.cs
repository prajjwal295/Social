using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Conversation.Commands;
using Social.Application.Models;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.ConversationAggreagate;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Social.Application.Conversation.CommandHandlers
{
    internal class CreateConversationCommandHandler
        : IRequestHandler<CreateConversationCommand, OperationResult<Domain.Aggregates.ConversationAggreagate.Conversation>>
    {
        private readonly DataContext _context;

        public CreateConversationCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Domain.Aggregates.ConversationAggreagate.Conversation>> Handle(
            CreateConversationCommand request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<Domain.Aggregates.ConversationAggreagate.Conversation>();

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                Domain.Aggregates.ConversationAggreagate.Conversation conversation;

                if (request.Participants.Count == 1 && request.Name == null)
                {
                    var participantId = request.Participants[0];
                    var existingConversation = await _context.Conversations
                        .Include(c => c.Participant)
                        .Where(c => !c.IsGroup &&
                                    c.Participant.Any(p => p.UserProfileId == request.CreatedBy) &&
                                    c.Participant.Any(p => p.UserProfileId == participantId))
                        .FirstOrDefaultAsync(cancellationToken);

                    if (existingConversation != null)
                    {
                        result.Payload = existingConversation;
                        return result;
                    }

                    conversation = Domain.Aggregates.ConversationAggreagate.Conversation.CreateOneToOneConversation(request.CreatedBy, participantId);

                    await _context.Conversations.AddAsync(conversation, cancellationToken);
                }
                else
                {
                    conversation = Domain.Aggregates.ConversationAggreagate.Conversation.CreateConversation(
                        request.Name,
                        request.PhotoUrl,
                        request.CreatedBy);

                    await _context.Conversations.AddAsync(conversation, cancellationToken);

                    foreach (var participantId in request.Participants)
                    {
                        var participant = ConversationParticipant.CreateConversationParticipant(
                            conversation.ConversationId,
                            participantId);

                        conversation.AddParticipants(participant);
                        await _context.ConversationParticipants.AddAsync(participant, cancellationToken);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                result.Payload = conversation;
                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                result.Errors.Add(new Error
                {
                    Code = Enums.ErrorCode.ServerError,
                    Message = ex.Message
                });

                return result;
            }
        }
    }
}
