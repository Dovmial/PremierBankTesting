using Infrastructure.Models.Bank;
using PremierBankTesting.DTOs;

namespace PremierBankTesting.Mapping
{
    public static class TransactionDtoExt
    {
        public static BankTransaction ToEntity(
            this BankTransactionResponse bankTransactionResponse,
            int userId)
            => new BankTransaction
            {
                Id = bankTransactionResponse.Id,
                Amount = bankTransactionResponse.Amount,
                Comment = bankTransactionResponse.Comment,
                Timestamp = bankTransactionResponse.Timestamp,
                UserId = userId
            };

        public static List<BankTransaction> ToListEntities(
            this ICollection<BankTransactionResponse> transactionResponsesDTO,
            Dictionary<string, User> emailDict)
            => transactionResponsesDTO
                .Select(x => x.ToEntity(emailDict[x.UserEmail].Id))
                .ToList();

        public static BankTransactionResponse ToDto(this BankTransaction entity)
            => new(
                entity.Id,
                entity.Amount,
                entity.Comment,
                entity.Timestamp,
                entity.User.Email);

        public static List<BankTransactionResponse> ToListResponses(
            this ICollection<BankTransaction> entities)
            => entities
            .Select(x => x.ToDto())
            .ToList();
    }
}
