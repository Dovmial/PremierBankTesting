namespace PremierBankTesting.Exceptions
{
    public class BadRequestException(string error): Exception(error)
    {
    }
}
