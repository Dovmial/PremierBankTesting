using System.ComponentModel.DataAnnotations;

namespace PremierBankTesting.DTOs
{
    public record BankTransactionResponse(
        Guid Id,
        decimal Amount,
        string Comment,

        [Required]
    DateTime Timestamp,

        [Required]
        [StringLength(50)] 
    string UserEmail);
}
