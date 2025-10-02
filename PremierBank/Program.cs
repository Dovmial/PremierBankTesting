using DotNetEnv;
using Infrastructure.Extensions;
using Microsoft.OpenApi.Models;
using PremierBankTesting.Clients.Bank;
using PremierBankTesting.Services.Implementations;
using PremierBankTesting.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);
Env.Load();
builder.Services.AddInfrastructureLayer();

builder.Services.AddControllers();

builder.Services
    .AddScoped<IBankApiClient, BankApiClient>()
    .AddScoped<IBankFacadeService, BankFacadeService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Документация Bank Premier API",
        Version = "v1",
        Description = "Описание API сервиса банка."
    });
    options.TagActionsBy(api => [api.GroupName]);
    options.DocInclusionPredicate((version, desc) => true);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.ApplyMigrationsAsync();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();