using Infrastructure.DbContexts;
using Infrastructure.Helpers;
using Infrastructure.Repositories.Implementations;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class DIInfrastructure
    {
        public static IServiceCollection AddInfrastructureLayer(
            this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("POSTGRESS_CONNECTION_STRIG_PREMIERDB");
            services.AddDbContext<NpgDbContext>(options =>
                options.UseNpgsql(connectionString));

            SetDbContext(services);
            ServiceRegistrations(services);

            return services;
        }

        private static void ServiceRegistrations(IServiceCollection services)
            => services
                .AddScoped<IBankTransactioinRepository, BankTransactionRepository>()
                .AddScoped<IBankUserRepository, BankUserRepository>()
                .AddSingleton<IHasher, Hasher>();

        private static void SetDbContext(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("POSTGRESS_CONNECTION_STRIG_PREMIERDB");
            services.AddDbContext<NpgDbContext>(options =>
                options.UseNpgsql(connectionString));
        }

        public static async Task<IApplicationBuilder> ApplyMigrations(this IApplicationBuilder app)
        {
            await using var scope = app.ApplicationServices.CreateAsyncScope();
            using var dbContex = scope.ServiceProvider.GetRequiredService<NpgDbContext>();
            await dbContex.Database.MigrateAsync();
            return app;
        }
    }
}
