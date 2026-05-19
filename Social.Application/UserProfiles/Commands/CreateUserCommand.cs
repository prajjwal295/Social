using MediatR;
using Social.Application.Models;
using Social.Domain.Aggregates.UserProfileAggegate;

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
        public string ProfilePicutreUrl { get; private set; }
        public string ProfilePicturePublicId { get; set; }
    }
}
