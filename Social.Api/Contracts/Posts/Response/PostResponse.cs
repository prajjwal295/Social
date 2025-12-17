using Microsoft.VisualBasic;
using Social.Domain.Aggregates.PostAggregate;
using System.Xml.Linq;

namespace Social.Api.Contracts.Posts.Response
{
    public class PostResponse
    {
        public Guid PostId { get;  set; }
        public Guid UserProfileId { get;  set; }
        public string TextContent { get;  set; }
        public DateTime CreatedDate { get;  set; }
        public DateTime LastModified { get;  set; }
    }
}
