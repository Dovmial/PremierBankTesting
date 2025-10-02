
using Infrastructure.DbContexts;
using Infrastructure.Models.Bank;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementations
{
    public sealed class BankUserRepository(NpgDbContext dbContext) : IBankUserRepository
    {
        public async Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateRangeAsync(ICollection<User> users, CancellationToken cancellationToken)
        {
            await dbContext.AddRangeAsync(users, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<ICollection<User>> GetAllAsync(CancellationToken cancellationToken)
            => await dbContext.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);

        public async Task<ICollection<User>> GetUsersByEmailsAsync(ICollection<string> emails, CancellationToken cancellationToken)
        => await dbContext.Users
                .Where(u => emails.Contains(u.Email))
                .ToListAsync(cancellationToken);
    }
}
