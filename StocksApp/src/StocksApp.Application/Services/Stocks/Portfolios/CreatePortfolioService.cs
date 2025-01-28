using Microsoft.Extensions.Logging;
using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Portfolios;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Interfaces;

namespace StocksApp.Application.Services.Stocks.Portfolios
{
    public class CreatePortfolioService : ICreatePortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ILogger<CreatePortfolioService> _logger;

        public CreatePortfolioService(IPortfolioRepository portfolioRepository, ILogger<CreatePortfolioService> logger)
        {
            _portfolioRepository = portfolioRepository;
            _logger = logger;
        }
        public async Task<PortfolioResponse> CreatePortfolioAsync(string userId, string portfolioName)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (string.IsNullOrWhiteSpace(portfolioName))
            {
                throw new ArgumentNullException(nameof(portfolioName));
            }

            Portfolio portfolio = new()
            {
                UserId = userId,
                Name = portfolioName
            };
            var result = await _portfolioRepository.CreatePortfolioAsync(portfolio);
            return result.ToPortfolioResponse();
        }
    }
}
