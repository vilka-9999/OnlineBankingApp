using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApp.Models
{
    public class User
    {
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Advisor is required")]
        public int? AdvisorId { get; set; }

        [ValidateNever]
        public Advisor Advisor { get; set; } = null!;

    }
}
