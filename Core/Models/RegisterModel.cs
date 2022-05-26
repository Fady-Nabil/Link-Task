using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class RegisterModel
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}