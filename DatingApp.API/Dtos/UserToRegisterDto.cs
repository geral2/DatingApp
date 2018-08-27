using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserToRegisterDto
    {
        [Required]
        public string username { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 10 characters")]
        public string password { get; set; }
    }
}