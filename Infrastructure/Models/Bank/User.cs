
namespace Infrastructure.Models.Bank
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; //всё ФИО для простоты
        public string Email { get; set; } = null!;
        public string HashPassword { get; set; } = null!;
        public bool IsDeleted { get;set; } //пометка удаления
        public ICollection<BankTransaction> Transactions { get; set; } = [];

    }
}
