using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineBankingApp.Models.LogInModels;

namespace OnlineBankingApp.Models
{
    public class OnlineBankingAppContext : DbContext
    {
        public OnlineBankingAppContext(DbContextOptions<OnlineBankingAppContext> options)
            : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Advisor> Advisors { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Transfer> Transfers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FirstName = "name1",
                    LastName = "name2",
                    Email = "1@gmail.com",
                    Username = "TestUU",
                    Password = "TestUP",
                    AdvisorId = 1,
                } 
            );

            modelBuilder.Entity<Advisor>().HasData(
                new Advisor
                {
                    AdvisorId = 1,
                    AdvisorName = "TestAN",
                    ClientsNumber = 1,
                }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = "TestAccId",
                    AccountBalance = 1,
                    AccountType = "Saving",
                    UserId = 1,
                    BankId = 1,
                },
                new Account
                {
                    AccountId = "TestAccId1",
                    AccountBalance = 1,
                    AccountType = "Saving",
                    UserId = 1,
                    BankId = 1,
                }
            );

            modelBuilder.Entity<Bank>().HasData(
                new Bank
                {
                    BankId = 1,
                    BankName = "BoA",
                    BankCountry = "USA",
                    BankType = "testBT"
                }
            );

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.SenderAccount)
                .WithMany()
                .HasForeignKey(t => t.SenderAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.ReceiverAccount)
                .WithMany()
                .HasForeignKey(t => t.ReceiverAccountId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transfer>().HasData(
                new Transfer
                {
                    TransferId = 1,
                    TransferAmount = 1,
                    SenderAccountId = "TestAccId",
                    ReceiverAccountId = "TestAccId1"
                }
            );
        }
        public DbSet<OnlineBankingApp.Models.LogInModels.LoginViewModel> LoginViewModel { get; set; } = default!;
        public DbSet<OnlineBankingApp.Models.LogInModels.RegistrationViewModel> RegistrationViewModel { get; set; } = default!;
    }
}
