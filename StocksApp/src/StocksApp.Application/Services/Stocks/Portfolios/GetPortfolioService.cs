using Microsoft.Extensions.Logging;
using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Portfolios;
using StocksApp.Domain.Exceptions;
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
        public async Task<PortfolioResponse?> GetPortfolioByIdAsync(string userId, Guid portfolioId)
        {
            if (portfolioId == Guid.Empty)
            {
                throw new ArgumentException("Portfolio ID cannot be empty", nameof(portfolioId));
            }

            var portfolio = await _portfolioRepository.GetPortfolioAsync(portfolioId);

            if (portfolio == null)
            {
                return null;
            }
            if (portfolio.UserId != userId)
            {
                throw new UnauthorizedResourceAccessException("User does not have access to this portfolio.");
            }
            return portfolio?.ToPortfolioResponse();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PortfolioResponse>?> GetPortfoliosAsync(string userId)
        {
            var portfolios = await _portfolioRepository.GetPortfoliosByUserAsync(userId);
            return portfolios?.Select(p => p.ToPortfolioResponse());
        }
    }
}
