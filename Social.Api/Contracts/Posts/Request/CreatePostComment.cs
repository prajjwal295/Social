using System.ComponentModel.DataAnnotations;

namespace Social.Api.Contracts.Posts.Request
{
    public class CreatePostComment
    {
        [Required]
        [MinLength(1)]
        [MaxLength(100)]
        public string CommentText { get;  set; }

        [Required]
        public string UserProfileId { get;  set; }
    }
}