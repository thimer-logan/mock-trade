using Microsoft.Extensions.DependencyInjection;
using StocksApp.Application.Interfaces.Authentication;
using StocksApp.Application.Interfaces.Finnhub;
using StocksApp.Application.Interfaces.Stocks.Portfolios;
using StocksApp.Application.Services.Authentication;
using StocksApp.Application.Services.Finnhub;
using StocksApp.Application.Services.Stocks.Portfolios;

namespace StocksApp.Application
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
            services.AddScoped<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
            services.AddScoped<IFinnhubStockSearchService, FinnhubStockSearchService>();
            services.AddScoped<IFinnhubStocksService, FinnhubStocksService>();

            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IGetPortfolioService, GetPortfolioService>();
            services.AddScoped<ICreatePortfolioService, CreatePortfolioService>();

            return services;
        }
    }
}
