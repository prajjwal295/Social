using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.UserProfileAggegate;


namespace Social.Application.UserProfiles.Queries
{
    public class GetAllUserProfiles : IRequest<OperationResult<List<UserProfile>>>
    {
    }
}
