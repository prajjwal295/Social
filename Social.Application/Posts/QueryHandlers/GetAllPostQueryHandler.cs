using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Queries;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.PostAggregate;

namespace Social.Application.Posts.QueryHandlers
{
    internal class GetAllPostQueryHandler : IRequestHandler<GetAllPosts, OperationResult<List<Post>>>
    {
        private readonly DataContext _context;

        public GetAllPostQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<Post>>> Handle(GetAllPosts request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<Post>>();

            try
            {
                var userProfileId = request.UserProfileId;

                if(userProfileId == null)
                {
                    var posts = await _context.Posts.ToListAsync();
                    result.Payload = posts;
                }
                else
                {
                    var postIds = await _context.UserFeed
                    .Where(uf => uf.UserProfileId == userProfileId)
                    .Include(uf => uf.FeedItems)
                    .SelectMany(uf => uf.FeedItems)
                    .Select(fi => fi.PostId)
                    .ToListAsync(cancellationToken);

                    var posts = await _context.Posts.
                        Where(p => postIds.Contains(p.PostId)).ToListAsync(cancellationToken);

                    result.Payload = posts;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result.IsError = true;
                result.Errors.Add(new Error
                {
                    Code = ErrorCode.UnknownError,
                    Message = ex.Message,
                });
            }

            return result;
        }
    }
}
