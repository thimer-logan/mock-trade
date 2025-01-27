using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Transactions;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Exceptions;
using StocksApp.Domain.Interfaces;

namespace StocksApp.Application.Services.Stocks.Transactions
{
    public class SellStockService : ISellStockService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly IHoldingRepository _holdingRepository;

        public SellStockService(ITransactionRepository transactionRepository, IPortfolioRepository portfolioRepository, IFinnhubRepository finnhubRepository, IHoldingRepository holdingRepository)
        {
            _transactionRepository = transactionRepository;
            _portfolioRepository = portfolioRepository;
            _finnhubRepository = finnhubRepository;
            _holdingRepository = holdingRepository;
        }

        public async Task<TransactionResponse> SellStockAsync(string userId, Guid portfolioId, SellStockRequest sellStockReq)
        {
            var portfolio = await _portfolioRepository.GetPortfolioAsync(portfolioId);
            if (portfolio == null)
            {
                throw new NotFoundException("Portfolio does not exist.");
            }
            if (portfolio.UserId != userId)
            {
                throw new UnauthorizedResourceAccessException("User does not have access to this portfolio.");
            }

            // Verify the user has enough shares to sell
            var holding = await _holdingRepository.GetHoldingByTicker(portfolioId, sellStockReq.Ticker);
            if (holding == null || sellStockReq.Quantity > holding.Quantity)
            {
                throw new InsufficientSharesException("Insufficient shares to sell stock.");
            }

            // Get the current price of the stock
            var quote = await _finnhubRepository.GetStockPriceQuoteAsync(sellStockReq.Ticker);
            if (quote == null)
            {
                throw new NotFoundException("Stock does not exist.");
            }

            // Create the transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                PortfolioId = portfolioId,
                Type = TransactionType.Sell,
                Ticker = sellStockReq.Ticker,
                Quantity = sellStockReq.Quantity,
                Price = (decimal)quote.c,
                Timestamp = DateTime.UtcNow
            };

            await _transactionRepository.CreateTransactionAsync(transaction);

            // Update the holding
            holding.Quantity -= sellStockReq.Quantity;
            holding.LastUpdated = DateTime.UtcNow;
            await _holdingRepository.UpdateHoldingAsync(holding);

            return transaction.ToTransactionResponse();
        }
    }
}
