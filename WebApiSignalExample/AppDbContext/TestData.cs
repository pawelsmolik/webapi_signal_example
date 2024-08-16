namespace WebApiSignalExample
{
    public class TestData(AppDbContext ctx)
    {
        private readonly AppDbContext _ctx = ctx;

        public void GenerateData() { 
            _ctx.Clients.Add(new Client { Id = 1, FirstName = "Jan", LastName = "Nowak" });
            _ctx.Clients.Add(new Client { Id = 2, FirstName = "Michał", LastName = "Kowalski" });
            _ctx.Clients.Add(new Client { Id = 3, FirstName = "Pan", LastName = "Tadeusz" });
            _ctx.Clients.Add(new Client { Id = 4, FirstName = "Mistrz", LastName = "Joda" });
            _ctx.Clients.Add(new Client { Id = 5, FirstName = "Jan", LastName = "Kazimierz" });

            _ctx.Accounts.Add(new Account() { Id = 1, ClientId = 1, AccountNumber = new Random().Next(100000000) });
            _ctx.Accounts.Add(new Account() { Id = 2, ClientId = 2, AccountNumber = new Random().Next(100000000) });
            _ctx.Accounts.Add(new Account() { Id = 3, ClientId = 3, AccountNumber = new Random().Next(100000000) });
            _ctx.Accounts.Add(new Account() { Id = 4, ClientId = 4, AccountNumber = new Random().Next(100000000) });

            _ctx.SaveChanges();
        }
    }
}
