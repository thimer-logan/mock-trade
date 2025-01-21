namespace StocksApp.Domain.ValueObjects
{
    public class StockSearch
    {
        /// <summary>
        /// A descriptive name for the symbol.
        /// Example: "APPLE INC"
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// A symbol formatted for display.
        /// Example: "AAPL"
        /// </summary>
        public string DisplaySymbol { get; set; } = string.Empty;

        /// <summary>
        /// The actual stock symbol/ticker.
        /// Example: "AAPL"
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// The type of security.
        /// Example: "Common Stock"
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}
