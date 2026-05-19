using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Infrastructure.Messaging.Events
{
    public record PostLikedEvent(
        Guid PostId,
        Guid LikedByUserId,
        Guid PostOwnerId
    );
}
