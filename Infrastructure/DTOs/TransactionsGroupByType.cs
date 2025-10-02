using Infrastructure.Models.Bank;
namespace Infrastructure.DTOs
{
    public record TransactionsGroupByType(string Type, List<BankTransaction> Transactions);
}
