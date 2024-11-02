using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApp.Models
{
    public class Bank
    {

        public int? BankId { get; set; }

        [Required(ErrorMessage = "Bank Name is required")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Bank Country is required")]
        public string BankCountry { get; set; }

        [Required(ErrorMessage = "Bank Type is required")]
        public string BankType { get; set; }
    }
}
