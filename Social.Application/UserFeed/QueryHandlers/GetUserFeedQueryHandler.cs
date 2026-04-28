using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.DTO;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserFeed.Queries;
using Social.DAL.DbContext;

namespace Social.Application.UserFeeds.QueryHandlers
{
    internal class GetUserFeedQueryHandler
        : IRequestHandler<GetUserFeedQuery, OperationResult<List<FeedDto>>>
    {
        private readonly DataContext _context;

        public GetUserFeedQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<FeedDto>>> Handle(
            GetUserFeedQuery request,
            CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<FeedDto>>();

            try
            {
                var userId = request.UserId;

                // 1. NORMAL FEED (fanout-on-write)
                var userFeed = await _context.UserFeed
                    .FirstOrDefaultAsync(x => x.UserProfileId == userId, cancellationToken);

                if (userFeed == null)
                {
                    return new OperationResult<List<FeedDto>>
                    {
                        Payload = new List<FeedDto>()
                    };
                }

                var normalFeed = userFeed
                                .FeedItems
                                .Select(x => new FeedDto
                                {
                                    PostId = x.PostId,
                                    AuthorId = userFeed.UserProfileId, 
                                    CreatedAt = x.CreatedAt,
                                    Source = "Normal"
                                }).ToList();

                // 2. FOLLOWING LIST
                var followees = await _context.Followers
                    .Where(x => x.FollowerId == userId && x.UnfollowedAt == null)
                    .Select(x => x.FolloweeId)
                    .ToListAsync(cancellationToken);

                // 3. CELEBRITY POSTS (fanout-on-read)
                var celebrityFeed = await _context.CelebrityPostCache
                    .Where(x => followees.Contains(x.UserProfileId))
                    .Select(x => new FeedDto
                    {
                        PostId = x.PostId,
                        AuthorId = x.UserProfileId,
                        CreatedAt = x.CreatedAt,
                        Source = "Celebrity"
                    })
                    .ToListAsync(cancellationToken);

                // 4. MERGE + SORT (timeline logic)
                var timeline = normalFeed
                    .Concat(celebrityFeed)
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(50)
                    .ToList();

                result.Payload = timeline;
                return result;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                result.Errors.Add(new Error
                {
                    Code = ErrorCode.ServerError,
                    Message = ex.Message
                });

                return result;
            }
        }
    }
}