using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApp.Models.LogInModels
{
    [Keyless]
    public class RegistrationViewModel
    {

        [Required(ErrorMessage = "First Name is required")]
        public String FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter the valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string ConfirmPaword { get; set; }
    }
}
