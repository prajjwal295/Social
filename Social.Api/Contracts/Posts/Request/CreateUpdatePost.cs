using System.ComponentModel.DataAnnotations;

namespace Social.Api.Contracts.Posts.Request
{
    public class CreateUpdatePost
    {
        [Required]
        [MinLength(1)]
        [MaxLength(300)]
        public string TextContent { get; set; }
    }
}
