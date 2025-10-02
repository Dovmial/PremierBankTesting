using System;

namespace Infrastructure.Models.Bank;

public class BankTransaction
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime Timestamp { get; set; }
    public int UserId { get; set; }
    public bool IsProcessed { get; set; }

    public User User = null!;
}