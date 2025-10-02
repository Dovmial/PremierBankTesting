using Infrastructure.DTOs;
using PremierBankTesting.DTOs;

namespace PremierBankTesting.Mapping
{
    public static class DtoResponseMapper
    {
        public static UsersTotalAmountsLastMonthResponse ToResponse(this UsersTotalAmountsLastMonth dto)
            => new(dto.UserId, dto.Email, dto.TotalAmount);

        public static List<UsersTotalAmountsLastMonthResponse> ToResponse(this ICollection<UsersTotalAmountsLastMonth> dtos)
            =>  dtos.Select(d => d.ToResponse()).ToList();

        public static TransactionsGroupByTypeResponse ToResponse(this TransactionsGroupByType dto)
            => new(dto.Type, dto.Transactions.ToListResponses());

        public static ICollection<TransactionsGroupByTypeResponse> ToResponse(
            this ICollection<TransactionsGroupByType> dtos) 
            => dtos.Select(d => d.ToResponse()).ToList();
    }
}
