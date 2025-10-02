using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PremierBankTesting.DTOs
{
    public record SetProcessedQuery(
         [Required] ICollection<Guid> Guids,
         bool IsProcessed = true);
}
