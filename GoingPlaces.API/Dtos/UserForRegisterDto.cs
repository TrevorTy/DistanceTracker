using System;
using System.ComponentModel.DataAnnotations;

namespace GoingPlaces.API.Dtos
{
    public class UserForRegisterDto
    {
        //Data Annotation to check for user validation
        [Required]
        public string Username { get; set; }
        //
        [Required]
       // [EmailAddress]
       //This only works because of [ApiController] attribute above the AuthController class
         [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
       // [Required]
      // public DateTime DateOfBirth { get; set; }
    }
}