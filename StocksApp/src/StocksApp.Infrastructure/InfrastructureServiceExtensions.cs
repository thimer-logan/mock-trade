using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StocksApp.Domain.Interfaces;
using StocksApp.Infrastructure.Repositories;

namespace StocksApp.Infrastructure
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IHoldingRepository, HoldingRepository>();
            services.AddScoped<IWatchlistRepository, WatchlistRepository>();

            services.AddTransient<IFinnhubRepository, FinnhubRepository>();
            services.AddHttpClient<IFinnhubRepository, FinnhubRepository>(client =>
            {
                var baseUrl = configuration["Finnhub:BaseUrl"]; client.BaseAddress = new Uri(baseUrl ?? "https://finnhub.io");
            });

            return services;
        }
    }
}
