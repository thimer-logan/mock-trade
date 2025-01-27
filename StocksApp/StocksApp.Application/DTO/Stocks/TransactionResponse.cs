using StocksApp.Domain.Entities.Stocks;

namespace StocksApp.Application.DTO.Stocks
{
    public class TransactionResponse
    {
        public Guid Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime TransactionDate { get; set; }
        public TransactionType Type { get; set; }
    }

    public static class TransactionResponseExtensions
    {
        public static TransactionResponse ToTransactionResponse(this Transaction transaction)
        {
            return new TransactionResponse
            {
                Id = transaction.Id,
                Ticker = transaction.Ticker,
                Quantity = transaction.Quantity,
                Price = transaction.Price,
                TransactionDate = transaction.Timestamp,
                Type = transaction.Type
            };
        }
    }
}
