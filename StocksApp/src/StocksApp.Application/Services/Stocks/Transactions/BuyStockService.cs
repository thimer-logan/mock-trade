using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Transactions;
using StocksApp.Domain.Entities.Stocks;
using StocksApp.Domain.Exceptions;
using StocksApp.Domain.Interfaces;

namespace StocksApp.Application.Services.Stocks.Transactions
{
    public class BuyStockService : IBuyStockService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly IHoldingRepository _holdingRepository;

        public BuyStockService(ITransactionRepository transactionRepository, IPortfolioRepository portfolioRepository, IFinnhubRepository finnhubRepository, IHoldingRepository holdingRepository)
        {
            _transactionRepository = transactionRepository;
            _portfolioRepository = portfolioRepository;
            _finnhubRepository = finnhubRepository;
            _holdingRepository = holdingRepository;
        }

        public async Task<TransactionResponse> BuyStockAsync(string userId, Guid portfolioId, BuyStockRequest buyStockReq)
        {
            if (portfolioId == Guid.Empty)
            {
                throw new BadRequestException("Portfolio Id is required.");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException("User Id is required.");
            }
            if (string.IsNullOrEmpty(buyStockReq.Ticker))
            {
                throw new BadRequestException("Ticker is required.");
            }


            var portfolio = await _portfolioRepository.GetPortfolioAsync(portfolioId);
            if (portfolio == null)
            {
                throw new NotFoundException("Portfolio does not exist.");
            }
            if (portfolio.UserId != userId)
            {
                throw new UnauthorizedResourceAccessException("User does not have access to this portfolio.");
            }

            // Get the current price of the stock
            var quote = await _finnhubRepository.GetStockPriceQuoteAsync(buyStockReq.Ticker);
            if (quote == null)
            {
                throw new NotFoundException("Stock does not exist.");
            }

            // Create the transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                PortfolioId = portfolioId,
                Type = TransactionType.Buy,
                Ticker = buyStockReq.Ticker,
                Quantity = buyStockReq.Quantity,
                Price = (decimal)quote.c,
                Timestamp = DateTime.UtcNow
            };

            await _transactionRepository.CreateTransactionAsync(transaction);

            var holding = await _holdingRepository.GetHoldingByTicker(portfolioId, buyStockReq.Ticker);
            if (holding == null)
            {
                // Create new holding
                holding = new Holding
                {
                    Id = Guid.NewGuid(),
                    PortfolioId = portfolioId,
                    Ticker = buyStockReq.Ticker,
                    Quantity = buyStockReq.Quantity,
                    AverageCostBasis = (decimal)quote.c,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                };

                await _holdingRepository.CreateHoldingAsync(holding);
            }
            else
            {
                // Update existing holding
                var oldQty = holding.Quantity;
                var oldAvg = holding.AverageCostBasis;

                var newQty = oldQty + buyStockReq.Quantity;
                // Weighted average = (oldQty*oldAvg + newQty*price) / totalShares
                var newAvg = ((oldQty * oldAvg) + (buyStockReq.Quantity * (decimal)quote.c)) / newQty;

                holding.Quantity = newQty;
                holding.AverageCostBasis = newAvg;
                holding.LastUpdated = DateTime.UtcNow;

                await _holdingRepository.UpdateHoldingAsync(holding);
            }

            return transaction.ToTransactionResponse();
        }
    }
}
