using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.UserProfiles.Commands;
using Social.DAL.DbContext;

internal class UnfollowCommandHandler
    : IRequestHandler<UnfollowUserCommand, OperationResult<bool>>
{
    private readonly DataContext _context;

    public UnfollowCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<bool>> Handle(
        UnfollowUserCommand request,
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
                    Message = "You cannot unfollow yourself"
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
                    Message = "User to Unfollow does not exist"
                });
                return result;
            }

            var follow = await _context.Followers
                .FirstOrDefaultAsync(x =>
                    x.FollowerId == request.FollowerId &&
                    x.FolloweeId == request.FolloweeId &&
                    x.UnfollowedAt == null);

            if (follow == null)
            {
                result.IsError = true;
                result.Errors.Add(new Error
                {
                    Code = ErrorCode.NotFound,
                    Message = "Follow relationship not found"
                });
                return result;
            }

            // 🔥 mark unfollow
            follow.Unfollow();

            // 🔥 update follower count
            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(u => u.UserProfileId == request.FolloweeId);

            if (userProfile != null)
            {
                userProfile.DecrementFollowers();
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
                Message = ex.Message
            });

            return result;
        }
    }
}