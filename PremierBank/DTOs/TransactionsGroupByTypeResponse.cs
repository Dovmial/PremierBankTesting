namespace PremierBankTesting.DTOs
{
    public record TransactionsGroupByTypeResponse(string Type, List<BankTransactionResponse> transactions);

}
