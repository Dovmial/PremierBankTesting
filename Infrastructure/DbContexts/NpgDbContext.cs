

using Infrastructure.Models.Bank;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts
{
    public sealed class NpgDbContext(DbContextOptions options): DbContext(options)
    {
        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankTransaction>()
                .HasOne(bt =>  bt.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(bt => bt.UserId)
                .OnDelete(DeleteBehavior.SetNull); //вдруг транзакции долны быть всегда

            modelBuilder.Entity<User>()
                .Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
               .Property(u => u.Name)
               .IsRequired()
               .HasMaxLength(50);


            modelBuilder.Entity<BankTransaction>()
                .Property(bt => bt.Amount)
                .IsRequired();
            
            modelBuilder.Entity<BankTransaction>()
                .Property(bt => bt.Comment)
                .HasMaxLength(150);

            //Индексы 
            modelBuilder.Entity<BankTransaction>()
                .HasIndex(bt => bt.Timestamp);

            modelBuilder.Entity<BankTransaction>()
               .HasIndex(bt => new { bt.Timestamp , bt.UserId});

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
               .HasIndex(u => u.Name)
               .IsUnique();


        }
    }
}
