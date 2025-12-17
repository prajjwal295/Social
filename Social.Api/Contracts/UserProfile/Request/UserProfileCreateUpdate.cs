using System.ComponentModel.DataAnnotations;

namespace Social.Api.Contracts.UserProfile.Request
{
    public record UserProfileCreateUpdate
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FirstName { get;  set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string LastName { get;  set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get;  set; }

        public string Phone { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        public string CurrentCity { get; set; }
    }
}
