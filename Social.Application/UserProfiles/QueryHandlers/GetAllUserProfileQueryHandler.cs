using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserProfiles.Queries;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.UserProfileAggegate;

namespace Social.Application.UserProfiles.QueryHandlers
{
    internal class GetAllUserProfileQueryHandler
        : IRequestHandler<GetAllUserProfiles, OperationResult<List<UserProfile>>>
    {
        private readonly DataContext _context;

        public GetAllUserProfileQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<UserProfile>>> Handle(GetAllUserProfiles request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<UserProfile>>();

            try
            {
                var response = await _context.UserProfiles.ToListAsync(cancellationToken);
                result.Payload = response;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Errors.Add(new Error
                {
                    Code = ErrorCode.UnknownError,
                    Message = ex.Message
                });
            }

            return result;
        }
    }
}
