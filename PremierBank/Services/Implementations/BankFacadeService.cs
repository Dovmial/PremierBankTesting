
using Infrastructure.Helpers;
using Infrastructure.Repositories.Interfaces;
using PremierBankTesting.DTOs;
using PremierBankTesting.Exceptions;
using PremierBankTesting.Mapping;
using PremierBankTesting.Services.Interfaces;

namespace PremierBankTesting.Services.Implementations
{
    public sealed class BankFacadeService(
        IBankTransactioinRepository transactionRepository,
        IBankUserRepository userRepository,
        IHasher hasher)
        : IBankFacadeService
    {
        #region Transaction
        /// <summary>
        /// сохранение списка транзакций только для уже существующих пользователей.
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public async Task SaveRangeAsync(ICollection<BankTransactionResponse> transactions, CancellationToken cancellationToken)
        {
            //оставляем уникальные
            var uniqEmails = transactions
                .Select(x => x.UserEmail.ToLower())
                .Distinct()
                .ToList();

            //ищем пользователей
            var users = await userRepository.GetUsersByEmailsAsync(uniqEmails, cancellationToken);
            var existingsEmail = users.Select(u => u.Email);

            //получаем несуществующих из запроса транзакций(должны существовать все)
            var missings = uniqEmails.Except(existingsEmail).ToList();
            if (missings.Any())
                throw new BadRequestException($"ошибка сохранения, отсутсвуют пользователи: {string.Join("; ", missings)}");

            //нужны пользователи сопоставляем в словарь
            var emailDict = users.ToDictionary(x => x.Email);
            foreach (var email in uniqEmails)
                emailDict[email] = users.First(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            //маппим и на сохранение
            var entities = transactions.ToListEntities(emailDict);
            await transactionRepository.SaveRangeAsync(entities, cancellationToken);
        }

        /// <summary>
        /// Маркировка коллекции транзакций, как обработанные
        /// </summary>
        /// <param name="guids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public async Task SetProcessedUpdateAsync(ICollection<Guid> guids, bool isProcessed, CancellationToken cancellationToken)
        {
            if(guids is null || guids.Count == 0)
                throw new BadRequestException("список транзакций пуст");
            await transactionRepository.SetProcessedUpdateAsync(guids, isProcessed, cancellationToken);
        }

        public async Task<ICollection<BankTransactionResponse>> GetTransactions(bool isProcessed, CancellationToken cancellationToken)
        {
            var transactionEntities = await transactionRepository.GetTransactions(isProcessed, cancellationToken);
            return transactionEntities.ToListDto();
        }
        

        #endregion

        #region User
        public async Task CreateUserAsync(BankUserAddRequest request, CancellationToken cancellationToken)
        {
            var entity = request.ToEntity();
            entity.HashPassword = hasher.Hash(request.Password);
            await userRepository.CreateAsync(entity, cancellationToken);
        }

        public async Task<ICollection<BankUserGetResponse>> GetAll(CancellationToken cancellationToken)
        {
            var entities = await userRepository.GetAll(cancellationToken);
            var result = entities.Select(x => x.ToDto()).ToList();
            return result;
        }

        #endregion
    }
}
