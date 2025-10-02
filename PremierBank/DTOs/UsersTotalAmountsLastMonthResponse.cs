namespace PremierBankTesting.DTOs
{
    public record UsersTotalAmountsLastMonthResponse(
        int UserId, string Email, decimal TotalAmount);
}
