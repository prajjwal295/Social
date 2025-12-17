using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserProfiles.Queries;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.PostAggregate;
using Social.Domain.Aggregates.UserProfileAggegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.UserProfiles.QueryHandlers
{
    internal class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileById, OperationResult<UserProfile>>
    {
        private readonly DataContext _context;

        public GetUserProfileByIdQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<UserProfile>> Handle(GetUserProfileById request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<UserProfile>();
            try
            {
                var response = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserProfileId == request.UserProfileId);

                if (response is null)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = "The Given User Id is Not Found in Database"
                    });
                }
                else
                {
                    result.Payload = response;
                }
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
