using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace OnlineBankingApp.Models
{
    public class Transfer
    {
        public int? TransferId { get; set; }

        [Required(ErrorMessage = "Transfer Amount is required")]
        public int TransferAmount { get; set; }

        public DateTime TransferDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Sender Account is required")]
        public int? SenderAccountId { get; set; }

        [ValidateNever]
        public Account SenderAccount { get; set; } = null!;

        [Required(ErrorMessage = "Receiver Account is required")]
        public int? ReceiverAccountId { get; set; }

        [ValidateNever]
        public Account ReceiverAccount { get; set; } = null!;
    }
}
