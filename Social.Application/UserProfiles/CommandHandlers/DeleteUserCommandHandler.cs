using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserProfiles.Commands;
using Social.DAL.DbContext;

namespace Social.Application.UserProfiles.CommandHandlers
{
    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult<Unit>>
    {
        private readonly DataContext _context;

        public DeleteUserCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Unit>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Unit>();

            try
            {
                var userProfile = await _context.UserProfiles
                    .FirstOrDefaultAsync(x => x.UserProfileId == request.UserProfileId, cancellationToken);

                if (userProfile == null)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = $"No UserProfile found with Id {request.UserProfileId}"
                    });
                    return result;
                }

                _context.UserProfiles.Remove(userProfile);
                await _context.SaveChangesAsync(cancellationToken);

                result.Payload = Unit.Value;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Errors.Add(new Error
                {
                    Code = ErrorCode.ServerError,
                    Message = ex.Message
                });
            }

            return result;
        }
    }
}
