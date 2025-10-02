using System.ComponentModel.DataAnnotations;

namespace PremierBankTesting.DTOs
{
    public record BankUserAddRequest(
        [Required]
        [StringLength(50)]
    string Name,
        [Required]
        [StringLength(50)]
    string Email,
        [Required]
        [MinLength(6)]
    string Password);
}