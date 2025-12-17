using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.UserProfileAggegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.UserProfiles.Commands
{
    public class CreateUserCommand  : IRequest<OperationResult<UserProfile>>
    {
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public string EmailAddress { get;  set; }
        public string Phone { get;  set; }
        public DateTime DateOfBirth { get;  set; }
        public string CurrentCity { get;  set; }
    }
}
