using MediatR;
using Social.Application.DTO;
using Social.Application.Models;

namespace Social.Application.UserFeed.Queries
{
    public class GetUserFeedQuery : IRequest<OperationResult<List<FeedDto>>>
    {
        public Guid UserId { get; set; }
    }
}
