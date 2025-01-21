using StocksApp.Domain.ValueObjects;

namespace StocksApp.Domain.Interfaces
{
    /// <summary>
    /// Repository that makes HTTP requests to finnhub.io
    /// </summary>
    public interface IFinnhubRepository
    {
        /// <summary>
        /// Returns company details such as company country, currency, exchange, IPO date, logo image, market capitalization, name of the company, phone number etc.
        /// </summary>
        /// <param name="symbol">Stock symbol to search</param>
        /// <returns>Returns a dictionary that contains details such as company country, currency, exchange, IPO date, logo image, market capitalization, name of the company, phone number etc.</returns>
        Task<CompanyProfile?> GetCompanyProfileAsync(string symbol);


        /// <summary>
        /// Returns stock price details such as current price, change in price, percentage change, high price of the day, low price of the day, open price of the day, previous close price
        /// </summary>
        /// <param name="symbol">Stock symbol to search</param>
        /// <returns>Returns a dictionary that contains details such as current price, change in price, percentage change, high price of the day, low price of the day, open price of the day, previous close price</returns>
        Task<StockQuote?> GetStockPriceQuoteAsync(string symbol);


        /// <summary>
        /// Returns list of all stocks supported by an exchange (default: US)
        /// </summary>
        /// <returns>List of stocks</returns>
        Task<IEnumerable<Stock>?> GetStocksAsync();


        /// <summary>
        /// Returns list of matching stocks based on the given stock symbol
        /// </summary>
        /// <param name="symbol">Stock symbol to search</param>
        /// <returns>List of matching stocks</returns>
        Task<List<StockSearch>?> SearchStocksAsync(string symbol);
    }
}
