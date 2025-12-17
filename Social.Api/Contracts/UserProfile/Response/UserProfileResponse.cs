using Social.Domain.Aggregates.UserProfileAggegate;

namespace Social.Api.Contracts.UserProfile.Response
{
    public class UserProfileResponse
    {
        public Guid UserProfileId { get;  set; }
        public string IdentityId { get;  set; }
        public BasicInformation BasicInfo { get;  set; }
        public DateTime DateCreated { get;  set; }
        public DateTime LastModified { get;  set; }
    }
}
