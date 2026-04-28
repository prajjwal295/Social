using MediatR;
using Social.Application.Models;

namespace Social.Application.UserProfiles.Commands
{
    public class UnfollowUserCommand : IRequest<OperationResult<bool>>
    {
        public Guid FollowerId { get; set; }
        public Guid FolloweeId { get; set; }
    }
}
