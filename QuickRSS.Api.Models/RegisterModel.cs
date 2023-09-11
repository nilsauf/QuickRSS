namespace QuickRSS.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
