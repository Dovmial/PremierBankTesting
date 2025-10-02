using Infrastructure.DbContexts;
using Infrastructure.DTOs;
using Infrastructure.Models.Bank;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementations
{
    public sealed class BankTransactionRepository(
        NpgDbContext dbContext) : IBankTransactioinRepository
    {
        public async Task<ICollection<BankTransaction>> GetTransactions(
            bool isProcessed, CancellationToken cancellationToken) 
            => await dbContext.BankTransactions
            .AsNoTracking()
            .Include(t => t.User)
            .Where(t => t.IsProcessed == isProcessed)
            .ToListAsync(cancellationToken);

        public async Task SaveRangeAsync(ICollection<BankTransaction> transactions, CancellationToken cancellationToken)
        {
            await dbContext.AddRangeAsync(transactions, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task SetProcessedUpdateAsync(ICollection<Guid> guids, bool isProcessed, CancellationToken cancellationToken)
         => await dbContext.BankTransactions
            .Where(x => guids.Contains(x.Id))
            .ExecuteUpdateAsync(setters => setters.SetProperty(t 
                => t.IsProcessed, isProcessed), cancellationToken);

        public async Task<ICollection<UsersTotalAmountsLastMonth>> GetSumAmountsByMonthForUsersAsync(bool isProcessed, CancellationToken cancellationToken)
        {
            var fromDate = DateTime.UtcNow.AddMonths(-1);
            return await dbContext.BankTransactions
                .AsNoTracking()
                .Where(t => t.IsProcessed == isProcessed && t.Timestamp >= fromDate)
                .GroupBy(t => new { t.UserId, t.User.Email})
                .Select(g => new UsersTotalAmountsLastMonth(
                    g.Key.UserId,
                    g.Key.Email,
                    g.Sum(t => t.Amount)))
                .ToListAsync(cancellationToken);
        }
    }
}
