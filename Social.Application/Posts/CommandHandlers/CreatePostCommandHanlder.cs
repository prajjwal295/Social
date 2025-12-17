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
    internal class CreatePostCommandHanlder : IRequestHandler<CreatePostCommand, OperationResult<Post>>
    {
        private readonly DataContext _context;

        public CreatePostCommandHanlder(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Post>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();
            try
            {
                var user = await _context.UserProfiles.FirstOrDefaultAsync(u => u.UserProfileId == request.UserProfileId);

                if (user == null)
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

                var post = Post.CreatePost(request.UserProfileId, request.TextContent);
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();

                result.Payload = post;

            }
            catch (PostNotValidException ex)
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
