using Social.Domain.Aggregates.UserProfileAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Aggregates.UserProfileAggegate
{
    public class UserProfile
    {
        private UserProfile()
        {

        }
        public Guid UserProfileId { get; private set; }
        public string IdentityId { get; private set; }
        public BasicInfo BasicInfo { get; private set; }
        public int FollowersCount { get; private set; }
        public List<RefreshToken> RefreshToken { get; set; } = new();
        public DateTime DateCreated { get; private set; }
        public DateTime LastModified { get; private set; }

        // factory method
        public static UserProfile CreateUserProfile(string identityId, BasicInfo basicInfo)
        {
            // todo:: implement error handling

            return new UserProfile
            {
                IdentityId = identityId,
                BasicInfo = basicInfo,
                FollowersCount = 0,
                DateCreated = DateTime.Now,
                LastModified = DateTime.Now
            };
        }

        //public method
        public void UpdateBasicInfo(BasicInfo basicInfo)
        {
            BasicInfo = basicInfo;
        }

        public void IncrementFollowers()
        {
            FollowersCount++;
        }

        public void DecrementFollowers()
        {
            if (FollowersCount > 0)
                FollowersCount--;
        }
    }
}
