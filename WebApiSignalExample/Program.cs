using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApiSignalExample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "AppDb");
});

builder.Services.AddScoped<TestData>();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");

app.MapGet("/clients", ([FromServices] AppDbContext ctx) =>
{
    var clients = ctx.Clients.ToArray();
    return clients;
})
.WithName("GetClients")
.WithOpenApi();

app.MapPost("/clients", ([FromServices] AppDbContext ctx, Client client) =>
{
    ctx.Clients.Add(client);
    ctx.SaveChanges();
    ctx.Accounts.Add(new Account() { Id = client.Id, ClientId = client.Id, AccountNumber = new Random().Next(100000000) });
    ctx.SaveChanges();
    return client.Id;
})
.WithName("AddClient")
.WithOpenApi();

app.MapDelete("/clients", ([FromServices] AppDbContext ctx, int id) =>
{
    var client = ctx.Clients.Where(a => a.Id == id).FirstOrDefault();
    if(client != null) ctx.Clients.Remove(client);
    var account = ctx.Accounts.Where(a => a.ClientId == id).FirstOrDefault();
    if(account != null) ctx.Accounts.Remove(account);
    ctx.SaveChanges();
    return id;
})
.WithName("RemoveClient")
.WithOpenApi();



app.MapGet("/accounts", ([FromServices] AppDbContext ctx) =>
{
    var clients = ctx.Accounts.ToArray();
    return clients;
})
.WithName("GeAccounts")
.WithOpenApi();

app.MapDelete("/accounts", ([FromServices] AppDbContext ctx, int id) =>
{
    var account = ctx.Accounts.Where(a => a.ClientId == id).First();
    ctx.Accounts.Remove(account);
    ctx.SaveChanges();
    return account.Id;
})
.WithName("RemoveAccount")
.WithOpenApi();

using (var scope = app.Services.CreateScope())
{
    var _emailRepository = scope.ServiceProvider.GetRequiredService<TestData>();
    _emailRepository.GenerateData();
}

app.Run();
