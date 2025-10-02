
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
        public async Task SaveRangeTransactionsAsync(ICollection<BankTransactionResponse> transactions, CancellationToken cancellationToken)
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
        public async Task<int> SetProcessedUpdateAsync(ICollection<Guid> guids, bool isProcessed, CancellationToken cancellationToken)
        {
            if(guids is null || guids.Count == 0)
                throw new BadRequestException("список транзакций пуст");
            return await transactionRepository.SetProcessedUpdateAsync(guids, isProcessed, cancellationToken);
        }

        public async Task<ICollection<BankTransactionResponse>> GetTransactionsAsync(bool isProcessed, CancellationToken cancellationToken)
        {
            var transactionEntities = await transactionRepository.GetTransactions(isProcessed, cancellationToken);
            return transactionEntities.ToListResponses();
        }

        public async Task<ICollection<UsersTotalAmountsLastMonthResponse>> GetSumAmountsByMonthForUsersAsync(bool isProcessed, CancellationToken cancellationToken)
            => (await transactionRepository
                .GetSumAmountsByMonthForUsersAsync(isProcessed, cancellationToken))
                .ToResponse();


        public async Task<ICollection<TransactionsGroupByTypeResponse>> GetGroupByType(bool isProcessed, CancellationToken cancellationToken)
        {
            var data = await transactionRepository.GetGroupByType(isProcessed, cancellationToken);
            var result =  data.ToResponse();
            return result;
        }

        #endregion

        #region User
        public async Task CreateUserAsync(BankUserAddRequest request, CancellationToken cancellationToken)
        {
            var entity = request.ToEntity(hasher);
            await userRepository.CreateAsync(entity, cancellationToken);
        }

        public async Task<ICollection<BankUserGetResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await userRepository.GetAllAsync(cancellationToken);
            var result = entities.Select(x => x.ToDto()).ToList();
            return result;
        }

        public async Task SaveRangeUsersAsync(ICollection<BankUserAddRequest> usersToAdd, CancellationToken cancellationToken)
        {
            var entities = usersToAdd.Select(x => x.ToEntity(hasher)).ToList();
            await userRepository.CreateRangeAsync(entities, cancellationToken);
        }

        #endregion
    }
}
