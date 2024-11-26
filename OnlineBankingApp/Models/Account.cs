using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBankingApp.Models
{
    public class Account
    {
        public string? AccountId { get; set; }

        [Required(ErrorMessage = "Account Balance is required")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AccountBalance { get; set; }

        [Required(ErrorMessage = "Account Type is required")]
        public string AccountType { get; set; }

        [Required(ErrorMessage = "User is required")]
        public int? UserId { get; set; }

        [ValidateNever]
        public User User { get; set; } = null!;

        [Required(ErrorMessage = "Bank is required")]
        public int BankId { get; set; }

        [ValidateNever]
        public Bank Bank { get; set; } = null!;

    }
}
