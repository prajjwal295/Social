using Social.Domain.Aggregates.UserProfileAggegate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Domain.Aggregates.UserProfileAggregate
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set;}
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        //Foriegn Key
        public Guid UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
