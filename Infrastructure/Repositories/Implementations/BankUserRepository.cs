
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
            await dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<User>> GetAll(CancellationToken cancellationToken)
            => await dbContext.Users
                .AsNoTracking()
                .ToListAsync();

        public async Task<ICollection<User>> GetUsersByEmailsAsync(ICollection<string> emails, CancellationToken cancellationToken)
        => await dbContext.Users
                .Where(u => emails.Contains(u.Email))
                .ToListAsync();
    }
}
