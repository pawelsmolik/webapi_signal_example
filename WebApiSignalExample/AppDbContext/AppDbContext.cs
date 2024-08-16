using Microsoft.EntityFrameworkCore;

namespace WebApiSignalExample
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Account> Accounts => Set<Account>();
    }

    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }

    public class Account
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int AccountNumber { get; set; }
    }
}
