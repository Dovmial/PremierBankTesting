
using Infrastructure.DTOs;
using PremierBankTesting.DTOs;

namespace PremierBankTesting.Services.Interfaces
{
    public interface IBankFacadeService
    {
        #region Transaction
        Task SaveRangeTransactionsAsync(ICollection<BankTransactionResponse> transactions, CancellationToken cancellationToken);
        Task<int> SetProcessedUpdateAsync(ICollection<Guid> guids, bool isProcessed, CancellationToken cancellationToken);
        Task<ICollection<BankTransactionResponse>> GetTransactionsAsync(bool isProcessed, CancellationToken cancellationToken);
        Task<ICollection<UsersTotalAmountsLastMonthResponse>> GetSumAmountsByMonthForUsersAsync(bool isProcessed, CancellationToken cancellationToken);
        Task<ICollection<TransactionsGroupByTypeResponse>> GetGroupByType(bool isProcessed, CancellationToken cancellationToken);
        #endregion

        #region User
        Task<ICollection<BankUserGetResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task CreateUserAsync(BankUserAddRequest request, CancellationToken cancellationToken);
        Task SaveRangeUsersAsync(ICollection<BankUserAddRequest> usersToAdd, CancellationToken cancellationToken);

        #endregion
    }
}
