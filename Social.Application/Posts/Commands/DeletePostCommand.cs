using MediatR;
using Social.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Posts.Commands
{
    public class DeletePostCommand : IRequest<OperationResult<Unit>>
    {
        public Guid PostId { get; set; }
    }
}
