using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.UserProfileAggegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Application.UserProfiles.Queries
{
    public class GetUserProfileById : IRequest<OperationResult<UserProfile>>
    {
        public Guid UserProfileId { get; set; }
    }
}
