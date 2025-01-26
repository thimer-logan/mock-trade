using Microsoft.Extensions.Logging;
using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Portfolios;
using StocksApp.Domain.Interfaces;

namespace StocksApp.Application.Services.Stocks.Portfolios
{
    /// <summary>
    /// Service to get a user's portfolio(s).
    /// </summary>
    public class GetPortfolioService : IGetPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ILogger<GetPortfolioService> _logger;

        public GetPortfolioService(IPortfolioRepository portfolioRepository, ILogger<GetPortfolioService> logger)
        {
            _portfolioRepository = portfolioRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<PortfolioResponse?> GetPortfolioByIdAsync(Guid portfolioId)
        {
            try
            {
                var portfolio = await _portfolioRepository.GetPortfolioAsync(portfolioId);
                return portfolio?.ToPortfolioResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolio.");
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PortfolioResponse>?> GetPortfoliosAsync(string userId)
        {
            try
            {
                var portfolios = await _portfolioRepository.GetPortfoliosByUserAsync(userId);
                return portfolios?.Select(p => p.ToPortfolioResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolios.");
            }

            return null;
        }
    }
}
