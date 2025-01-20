using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StocksApp.Application.Interfaces;
using System.Text.Json;

namespace StocksApp.Infrastructure.Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FinnhubRepository> _logger;
        private readonly string _apiKey;

        public FinnhubRepository(HttpClient httpClient, IConfiguration configuration, ILogger<FinnhubRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            // Pull the API key from user-secrets or environment variables
            _apiKey = configuration["Finnhub:ApiKey"] ?? throw new ArgumentNullException("Finnhub API key not found");
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfileAsync(string symbol)
        {
            var response = await _httpClient.GetAsync($"/api/v1/profile2?symbol={symbol}&token={_apiKey}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(content);

            if (responseDictionary == null)
            {
                throw new InvalidOperationException("Failed to deserialize response from Finnhub");
            }

            if (responseDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
            }

            return responseDictionary;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuoteAsync(string symbol)
        {
            var response = await _httpClient.GetAsync($"/api/v1/quote?symbol={symbol}&token={_apiKey}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(content);

            if (responseDictionary == null)
            {
                throw new InvalidOperationException("Failed to deserialize response from Finnhub");
            }

            if (responseDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
            }

            return responseDictionary;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocksAsync()
        {
            var response = await _httpClient.GetAsync($"/api/v1/stock/symbol&token={_apiKey}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            List<Dictionary<string, string>>? responseDictionary = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(content);

            if (responseDictionary == null)
            {
                throw new InvalidOperationException("Failed to deserialize response from Finnhub");
            }

            return responseDictionary;
        }

        public async Task<Dictionary<string, object>?> SearchStocksAsync(string symbol)
        {
            var response = await _httpClient.GetAsync($"/api/v1/search?q={symbol}&token={_apiKey}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(content);

            if (responseDictionary == null)
            {
                throw new InvalidOperationException("Failed to deserialize response from Finnhub");
            }

            if (responseDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
            }

            return responseDictionary;
        }
    }
}
