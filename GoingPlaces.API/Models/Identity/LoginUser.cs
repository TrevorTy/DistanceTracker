using System.ComponentModel.DataAnnotations;

namespace GoingPlaces.API.Models.Identity
{
    public class LoginUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}