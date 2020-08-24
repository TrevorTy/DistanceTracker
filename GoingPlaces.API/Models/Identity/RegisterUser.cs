using System.ComponentModel.DataAnnotations;

namespace GoingPlaces.API.Models.Identity
{
    public class RegisterUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}