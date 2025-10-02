using Infrastructure.Models.Bank;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IBankTransactioinRepository
    {
        Task SaveRangeAsync(ICollection<BankTransaction> transactions, CancellationToken cancellationToken);
        Task SetProcessedUpdateAsync(ICollection<Guid> guids, bool isProcessed, CancellationToken cancellationToken);

        Task<ICollection<BankTransaction>> GetTransactions(bool isProcessed, CancellationToken cancellationToken);
    }
}
