using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.PostAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Posts.Queries
{
    public class GetPostCommentById : IRequest<OperationResult<PostComment>>
    {
        public Guid PostId { get; set; }
        public Guid CommentId { get; set; }
    }
}
