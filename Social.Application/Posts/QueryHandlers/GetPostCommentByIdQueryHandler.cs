using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Enums;
using Social.Application.Models;
using Social.Application.Posts.Queries;
using Social.DAL.DbContext;
using Social.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Posts.QueryHandlers
{
    internal class GetPostCommentByIdQueryHandler : IRequestHandler<GetPostCommentById, OperationResult<PostComment>>
    {
        private readonly DataContext _context;

        public GetPostCommentByIdQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PostComment>> Handle(GetPostCommentById request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PostComment>();
            try
            {
                var post = await _context.Posts.
                    Include(p => p.Comments).
                    FirstOrDefaultAsync(p => p.PostId == request.PostId);

                if (post is null)
                {
                    result.IsError = true;
                    var error = new Error();
                    error.Message = "The Post Id is Incorrect";
                    error.Code = Enums.ErrorCode.NotFound;
                    result.Errors.Add(error);
                }
                else
                {
                    var comments = post.Comments.FirstOrDefault(c => c.CommentId == request.CommentId);

                    if (comments is null)
                    {
                        result.IsError = true;
                        var error = new Error();
                        error.Message = "The Comment Id is Incorrect";
                        error.Code = Enums.ErrorCode.NotFound;
                        result.Errors.Add(error);
                    }
                    else
                    {
                        result.Payload = comments;
                    }
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
