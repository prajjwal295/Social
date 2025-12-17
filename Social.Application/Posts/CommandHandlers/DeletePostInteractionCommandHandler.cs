using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Models;
using Social.Application.Posts.Commands;
using Social.DAL.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Posts.CommandHandlers
{
    internal class DeletePostInteractionCommandHandler : IRequestHandler<DeletePostInteractionCommand, OperationResult<Unit>>
    {
        private readonly DataContext _context;

        public DeletePostInteractionCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Unit>> Handle(DeletePostInteractionCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Unit>();
            try
            {
                var postInteraction = await _context.PostInteraction.FirstOrDefaultAsync(p => p.InteractionId == request.InteractionId);

                if(postInteraction == null)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Message = $"No Post Interaction found with Id {request.InteractionId}",
                        Code = Enums.ErrorCode.NotFound
                    };
                    result.Errors.Add(error);
                    return result;
                }

                if (postInteraction.UserProfileId != request.UserProfileId) {
                    result.IsError = true;
                    var error = new Error
                    {
                        Message = $"Unauthorized",
                        Code = Enums.ErrorCode.Unauthorized
                    };
                    result.Errors.Add(error);
                    return result;
                }

                _context.PostInteraction.Remove(postInteraction);
                await _context.SaveChangesAsync();
                result.Payload = Unit.Value;
            }
            catch(Exception ex)
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
