using MediatR;
using Social.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.Posts.Commands
{
    public class DeletePostInteractionCommand : IRequest<OperationResult<Unit>>
    {
        public Guid UserProfileId { get; set; }
        public Guid InteractionId { get; set; }
    }
}
