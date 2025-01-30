using StocksApp.Application.DTO.Stocks;
using StocksApp.Application.Interfaces.Stocks.Transactions;
using StocksApp.Domain.Exceptions;
using StocksApp.Domain.Interfaces;

namespace StocksApp.Application.Services.Stocks.Transactions
{
    public class GetTransactionService : IGetTransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public GetTransactionService(ITransactionRepository transactionRepository, IPortfolioRepository portfolioRepository)
        {
            _transactionRepository = transactionRepository;
            _portfolioRepository = portfolioRepository;
        }

        public async Task<TransactionResponse> GetTransactionByIdAsync(string userId, Guid portfolioId, Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new BadRequestException("Transaction Id is required.");
            }
            if (portfolioId == Guid.Empty)
            {
                throw new BadRequestException("Portfolio Id is required.");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException("User Id is required.");
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

            var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                throw new NotFoundException("Transaction does not exist.");
            }

            return transaction.ToTransactionResponse();
        }

        public async Task<IEnumerable<TransactionResponse>> GetTransactionsAsync(string userId, Guid portfolioId)
        {
            if (portfolioId == Guid.Empty)
            {
                throw new BadRequestException("Portfolio Id is required.");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException("User Id is required.");
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

            var transactions = await _transactionRepository.GetTransactionsByPortfolioAsync(portfolioId);
            return transactions.Select(t => t.ToTransactionResponse());
        }
    }
}
