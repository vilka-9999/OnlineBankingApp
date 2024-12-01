using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApp.Models
{
    public class Advisor
    {
        public int? AdvisorId { get; set; }

        [Required(ErrorMessage = "Advisor Name is required")]
        public string AdvisorName { get; set; }

        [Required(ErrorMessage = "Advisor Name is required")]
        [Phone(ErrorMessage = "Enter the valid Phone")]
        public string AdvisorPhone { get; set; }

        [Required(ErrorMessage = "Advisor Name is required")]
        [EmailAddress(ErrorMessage = "Enter the valid Email")]
        public string AdvisorEmail { get; set; }
        public int ClientsNumber { get; set; }

    }
}
