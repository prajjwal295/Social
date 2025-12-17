using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Models;
using Social.Application.Posts.Commands;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.PostAggregate;
using Social.Domain.Exceptions;
using Social.Domain.Validators.PostValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Posts.CommandHandlers
{
    internal class CreatePostCommentCommandHandler : IRequestHandler<CreatePostCommentCommand, OperationResult<PostComment>>
    {
        private readonly DataContext _context;

        public CreatePostCommentCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PostComment>> Handle(CreatePostCommentCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostComment>();
            try
            {
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

                var comment = PostComment.CreateComment(request.PostId, request.UserProfileId, request.CommentText);

                post.AddComment(comment);

                //why??
                _context.Posts.Update(post);


                await _context.SaveChangesAsync();

                result.Payload = comment;
            }
            catch (PostCommentNotValidException ex)
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
            catch (Exception ex)
            {
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
