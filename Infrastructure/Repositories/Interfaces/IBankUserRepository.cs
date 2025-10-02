

using Infrastructure.Models.Bank;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IBankUserRepository
    {
        Task CreateAsync(User user, CancellationToken cancellationToken);
        Task CreateRangeAsync(ICollection<User> users, CancellationToken cancellationToken);
        Task<ICollection<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<ICollection<User>> GetUsersByEmailsAsync(ICollection<string> emails, CancellationToken cancellationToken);
    }
}