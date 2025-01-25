using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StocksApp.Domain.Interfaces;
using StocksApp.Domain.ValueObjects;

namespace StocksApp.Infrastructure.Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FinnhubRepository> _logger;
        private readonly string _apiKey;

        JsonLoadSettings jsonLoadSettings = new JsonLoadSettings
        {
            CommentHandling = CommentHandling.Ignore,
            LineInfoHandling = LineInfoHandling.Ignore
        };

        public FinnhubRepository(HttpClient httpClient, IConfiguration configuration, ILogger<FinnhubRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            // Pull the API key from user-secrets or environment variables
            _apiKey = configuration["Finnhub:ApiKey"] ?? throw new ArgumentNullException("Finnhub API key not found");
        }

        public async Task<CompanyProfile?> GetCompanyProfileAsync(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            var response = await _httpClient.GetAsync($"/api/v1/stock/profile2?symbol={symbol.ToUpper()}&token={_apiKey}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new InvalidOperationException("Error fetching company data from Finnhub.");
            }

            if (content.Contains("\"error\""))
            {
                throw new InvalidOperationException($"Error fetching company data from Finnhub: {content}");
            }

            var company = JsonConvert.DeserializeObject<CompanyProfile>(content);

            return company;
        }

        public async Task<StockQuote?> GetStockPriceQuoteAsync(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            var response = await _httpClient.GetAsync($"/api/v1/quote?symbol={symbol.ToUpper()}&token={_apiKey}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new InvalidOperationException("Error fetching stock quote from Finnhub.");
            }

            if (content.Contains("\"error\""))
            {
                throw new InvalidOperationException($"Error fetching stock quote from Finnhub: {content}");
            }

            var quote = JsonConvert.DeserializeObject<StockQuote>(content);

            return quote;
        }

        public async Task<IEnumerable<Stock>?> GetStocksAsync()
        {
            var response = await _httpClient.GetAsync($"/api/v1/stock/symbol?exchange=US&token={_apiKey}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new InvalidOperationException("Error fetching stocks from Finnhub.");
            }

            if (content.Contains("\"error\""))
            {
                throw new InvalidOperationException($"Error fetching stocks from Finnhub: {content}");
            }

            var stockData = JsonConvert.DeserializeObject<List<Stock>>(content);

            return stockData;
        }

        public async Task<IEnumerable<StockSearch>?> SearchStocksAsync(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            var response = await _httpClient.GetAsync($"/api/v1/search?q={symbol.ToUpper()}&token={_apiKey}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new InvalidOperationException("Error fetching stocks from Finnhub.");
            }

            if (content.Contains("\"error\""))
            {
                throw new InvalidOperationException($"Error fetching stocks from Finnhub: {content}");
            }

            // Response has a different structure than the other endpoints
            // It has a "count" and "result" key that contains the search results
            var searchResults = JsonConvert.DeserializeObject<PaginatedResponse<StockSearch>>(content)?.Result;

            return searchResults;
        }
    }
}
