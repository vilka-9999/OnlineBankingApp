using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApp.Models.LogInModels
{
    [Keyless]
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Email or Username is required")]
        [Display(Name = "Email or Username")]
        public string EmailOrUsername { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
