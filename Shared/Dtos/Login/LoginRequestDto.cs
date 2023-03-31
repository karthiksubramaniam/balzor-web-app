using System.ComponentModel.DataAnnotations;

namespace balzor_web_app.Shared.Dtos.Login
{
    public class LoginRequestDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Password { get; set; }
    }
}