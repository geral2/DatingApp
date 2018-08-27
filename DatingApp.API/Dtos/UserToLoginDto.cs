using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserToLoginDto
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}