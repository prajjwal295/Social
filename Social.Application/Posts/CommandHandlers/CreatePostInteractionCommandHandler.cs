using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Models;
using Social.Application.Posts.Commands;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.PostAggregate;
using Social.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Posts.CommandHandlers
{
    internal class CreatePostInteractionCommandHandler : IRequestHandler<CreatePostInteractionCommand, OperationResult<PostInteraction>>
    {
        private readonly DataContext _context;

        public CreatePostInteractionCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PostInteraction>> Handle(CreatePostInteractionCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostInteraction>();
            try
            {
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserProfileId == request.UserProfileId);

                if (userProfile == null)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = Enums.ErrorCode.NotFound,
                        Message = "User Id Not Found"
                    };
                    result.Errors.Add(error);
                    return result;
                }

                var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == request.PostId);

                if (post is null)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Code = Enums.ErrorCode.NotFound,
                        Message = $"No Post Found with ID {request.PostId}"
                    };
                    result.Errors.Add(error);
                    return result;
                }

                var postInteraction = PostInteraction.CreateInteraction(request.PostId,request.UserProfileId, request.type);

                post.AddInteraction(postInteraction);
                await _context.SaveChangesAsync();

                result.Payload = postInteraction;
                return result;
            }
            catch (PostInteractionNotValidException ex)
            {
                result.IsError = true;
                ex.ValidationErrors.ForEach(er =>
                {
                    var error = new Error
                    {
                        Code = Enums.ErrorCode.ValidationError,
                        Message = er
                    };
                    result.Errors.Add(error);
                });
            }
            catch (Exception ex) {
                result.IsError = true;
                var error = new Error
                {
                    Code = Enums.ErrorCode.UnknownError,
                    Message = ex.Message
                };
                result.Errors.Add(error);
            }
            return result;
        }
    }
}
