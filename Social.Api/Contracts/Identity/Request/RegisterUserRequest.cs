using System.ComponentModel.DataAnnotations;

namespace Social.Api.Contracts.Identity.Request
{
    public class RegisterUserRequest
    {
        [Required]
        [EmailAddress]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string FirstName { get;  set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LastName { get;  set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get;  set; }

        [Required]
        [Phone]
        public string Phone { get;  set; }
        
        public IFormFile? ProfilePicture { get; set; }

        [Required]
        public DateTime DateOfBirth { get;  set; }
        public string? CurrentCity { get;  set; }
    }
}
