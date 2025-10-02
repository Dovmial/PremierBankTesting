

using Infrastructure.Models.Bank;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IBankUserRepository
    {
        Task CreateAsync(User user, CancellationToken cancellationToken);
        Task<ICollection<User>> GetAll(CancellationToken cancellationToken);
        Task<ICollection<User>> GetUsersByEmailsAsync(ICollection<string> emails, CancellationToken cancellationToken);
    }
}