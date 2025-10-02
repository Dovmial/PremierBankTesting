

using PremierBankTesting.DTOs;

namespace PremierBankTesting.Clients.Bank;

public interface IBankApiClient
{
    Task<List<BankTransactionResponse>> GetRecentTransactionsAsync(CancellationToken cancellationToken);
    Task<List<BankUserAddRequest>> UsersForAdd();
}