using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Application.Models;
using Social.Application.Posts.Commands;
using Social.DAL.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Posts.CommandHandlers
{
    internal class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, OperationResult<Unit>>
    {
        private readonly DataContext _context;

        public DeletePostCommandHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<Unit>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<Unit>();
            try
            {
                var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == request.PostId);

                if (post is null)
                {
                    result.IsError = true;
                    var error = new Error
                    {
                        Message = $"No Post found with Id {request.PostId}",
                        Code = Enums.ErrorCode.NotFound
                    };
                    result.Errors.Add(error);
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                result.Payload = Unit.Value;
            }
            catch (Exception ex)
            {
                result.IsError = true;
                var error = new Error
                {
                    Message = "Server Error",
                    Code = Enums.ErrorCode.ServerError
                };
                result.Errors.Add(error);
            }

            return result;
        }
    }
}
