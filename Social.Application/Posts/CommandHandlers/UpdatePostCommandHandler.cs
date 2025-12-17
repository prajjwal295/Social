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
    internal class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, OperationResult<Post>>
    {
        private readonly DataContext _context;

        public UpdatePostCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Post>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();
            try
            {
                var user = await _context.Posts.FirstOrDefaultAsync(u => u.UserProfileId == request.UserProfileId);
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

                var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == request.PostId);

                if (post == null)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Message = "Give Post Id is Not Found in Database",
                        Code = Enums.ErrorCode.NotFound
                    };

                    result.Errors.Add(error);
                }

                post.UpdatePostText(request.TextContent);
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
                        Message = er,
                        Code = Enums.ErrorCode.ValidationError
                    };
                    result.Errors.Add(error);
                }); 
            }
            catch (Exception ex) {
                result.IsError = true;
                var error = new Error
                {
                    Message = "Give Post Id is Not Found in Database",
                    Code = Enums.ErrorCode.NotFound
                };
                result.Errors.Add(error);
            }
            return result;
        }
    }
}
