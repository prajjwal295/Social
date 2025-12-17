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
    internal class GetPostCommentQueryHandler : IRequestHandler<GetPostComments, OperationResult<List<PostComment>>>
    {
        private readonly DataContext _context;

        public GetPostCommentQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<PostComment>>> Handle(GetPostComments request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<PostComment>>();
            try
            {
                var post = await _context.Posts
                    .Include(p => p.Comments)
                    .FirstOrDefaultAsync(p => p.PostId == request.PostId);


                if (post == null)
                {
                    result.IsError = true;
                    result.Errors.Add(new Error
                    {
                        Code = ErrorCode.NotFound,
                        Message = "The Given Post Id is Not Found in Database"
                    });
                }
                else
                {
                    result.Payload = post.Comments.ToList();
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
