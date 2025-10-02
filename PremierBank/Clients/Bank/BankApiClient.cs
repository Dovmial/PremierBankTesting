using System;
using PremierBankTesting.DTOs;

namespace PremierBankTesting.Clients.Bank;

public class BankApiClient :IBankApiClient
{
    public Task<List<BankTransactionResponse>> GetRecentTransactionsAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<BankTransactionResponse>
        {
            new(Guid.NewGuid(), 1000, "Пополнение", DateTime.UtcNow.AddMinutes(-1), "ivanov@test.com"),
            new(Guid.NewGuid(), 2000, "Оплата",     DateTime.UtcNow.AddMinutes(-2), "unknown@test.com"),
            new(Guid.NewGuid(), -500, "Списание",   DateTime.UtcNow.AddMinutes(-3), "unknown@test.com"),
            new(Guid.NewGuid(), 1000, "Оплата",     DateTime.UtcNow.AddDays(-3),    "ivanov@test.com"),
            new(Guid.NewGuid(), 1500, "Пополнение", DateTime.UtcNow.AddDays(-10),   "petrov@test.com"),
            new(Guid.NewGuid(), 500,  "Подарок",    DateTime.UtcNow.AddDays(-15),   "ivanov@test.com")
        });
    }
}