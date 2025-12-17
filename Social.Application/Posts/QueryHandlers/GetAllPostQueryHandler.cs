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
    internal class GetAllPostQueryHandler : IRequestHandler<GetAllPosts, OperationResult<List<Post>>>
    {
        private readonly DataContext _context;

        public GetAllPostQueryHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<Post>>> Handle(GetAllPosts request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<List<Post>>();

            try
            {
                var posts = await _context.Posts.ToListAsync();
                result.Payload = posts;
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
