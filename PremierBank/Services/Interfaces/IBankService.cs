
using PremierBankTesting.DTOs;
using System.Linq.Expressions;

namespace PremierBankTesting.Services.Interfaces
{
    public interface IBankFacadeService
    {
        #region Transaction
        Task SaveRangeAsync(ICollection<BankTransactionResponse> transactions, CancellationToken cancellationToken);
        Task SetProcessedUpdateAsync(ICollection<Guid> guids, bool isProcessed, CancellationToken cancellationToken);
        Task<ICollection<BankTransactionResponse>> GetTransactions(bool isProcessed, CancellationToken cancellationToken);
        #endregion

        #region User
        Task<ICollection<BankUserGetResponse>> GetAll(CancellationToken cancellationToken);
        Task CreateUserAsync(BankUserAddRequest request, CancellationToken cancellationToken);
        #endregion
    }
}
