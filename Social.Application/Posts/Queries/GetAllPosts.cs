using MediatR;
using Social.Application.Caching;
using Social.Application.Models;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application.Posts.Queries
{
    public class GetAllPosts : IRequest<OperationResult<List<Post>>>, ICacheable
    {
        public bool BypassCache => false;

        public string CacheKey => $"GetAllPost";

        public int SlidingExpirationInMinutes => 30;

        public int AbsoluteExpirationInMinutes => 60;
    }
}
