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
    internal class GetPostByIdQueryHanlder : IRequestHandler<GetPostById, OperationResult<Post>>
    {
        private readonly DataContext _context;

        public GetPostByIdQueryHanlder(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Post>> Handle(GetPostById request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Post>();
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == request.PostId);

                if (post is null)
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
                    result.Payload = post;
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
