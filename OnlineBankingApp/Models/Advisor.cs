using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApp.Models
{
    public class Advisor
    {
        public int? AdvisorId { get; set; }

        [Required(ErrorMessage = "Advisor Name is required")]
        public string AdvisorName { get; set; }

        public int ClientsNumber { get; set; }

    }
}
