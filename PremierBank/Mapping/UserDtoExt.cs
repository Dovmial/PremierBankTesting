using Infrastructure.Helpers;
using Infrastructure.Models.Bank;
using PremierBankTesting.DTOs;

namespace PremierBankTesting.Mapping
{
    public static class UserDtoExt
    {
        public static User ToEntity(this BankUserAddRequest request)
            => new User
            {
                Email = request.Email.ToLower(),
                Name = request.Name,
            };

        public static BankUserGetResponse ToDto(this User entity)
            => new(entity.Id, entity.Name, entity.Email);
    }
}
