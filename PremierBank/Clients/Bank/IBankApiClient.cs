

using PremierBankTesting.DTOs;

namespace PremierBankTesting.Clients.Bank;

public interface IBankApiClient
{
    public Task<List<BankTransactionResponse>> GetRecentTransactionsAsync(CancellationToken cancellationToken);
}