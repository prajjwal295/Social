using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserProfiles.Commands;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.FollowAggregate.Social.Domain.Aggregates.FollowAggregate;

internal class FollowCommandHandler: IRequestHandler<FollowUserCommand, OperationResult<bool>>
{
    private readonly DataContext _context;

    public FollowCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<bool>> Handle(
        FollowUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = new OperationResult<bool>();

        try
        {

            if (request.FollowerId == request.FolloweeId)
            {
                result.IsError = true;
                result.Errors.Add(new Error
                {
                    Code = ErrorCode.ValidationError,
                    Message = "You cannot follow yourself"
                });
                return result;
            }


            var followerExists = await _context.UserProfiles
    .AnyAsync(u => u.UserProfileId == request.FollowerId);

            if (!followerExists)
            {
                result.IsError = true;
                result.Errors.Add(new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = "Follower user does not exist"
                });
                return result;
            }

            var followeeExists = await _context.UserProfiles
                .AnyAsync(u => u.UserProfileId == request.FolloweeId);

            if (!followeeExists)
            {
                result.IsError = true;
                result.Errors.Add(new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = "User to follow does not exist"
                });
                return result;
            }

            var existing = await _context.Followers
                .FirstOrDefaultAsync(x =>
                    x.FollowerId == request.FollowerId &&
                    x.FolloweeId == request.FolloweeId);

            if (existing != null)
            {
                // already active follow
                if (existing.UnfollowedAt == null)
                {
                    result.Payload = true;
                    return result;
                }

                // re-follow
                existing.Reactivate();

                // increment follower count
                var userProfile = await _context.UserProfiles
                    .FirstOrDefaultAsync(u => u.UserProfileId == request.FolloweeId);

                if (userProfile != null)
                {
                    userProfile.IncrementFollowers();
                }
            }
            else
            {
                var follow = Follow.Create(
                    request.FollowerId,
                    request.FolloweeId);

                await _context.Followers.AddAsync(follow);

                var userProfile = await _context.UserProfiles
                    .FirstOrDefaultAsync(u => u.UserProfileId == request.FolloweeId);

                if (userProfile != null)
                {
                    userProfile.IncrementFollowers();
                }
            }

            await _context.SaveChangesAsync();

            result.Payload = true;
            return result;
        }
        catch (Exception ex)
        {
            result.IsError = true;
            result.Errors.Add(new Error
            {
                Code = ErrorCode.ServerError,
                Message = ex.Message,
            });

            return result;
        }
    }
}