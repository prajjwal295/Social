using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Posts.Commands
{
    public class UpdatePostCommand : IRequest<OperationResult< Post>>
    {
        public Guid PostId { get; set; }
        public Guid UserProfileId { get; set; }
        public string TextContent { get; set; }
    }
}
