using MediatR;
using Social.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.UserProfiles.Commands
{
    public class DeleteUserCommand : IRequest<OperationResult<Unit>>
    {
        public Guid UserProfileId;
    }
}
