namespace StocksApp.Domain.ValueObjects
{
    public class Stock
    {
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Full name/description of the stock.
        /// Example: "UAN POWER CORP"
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// A symbol formatted for display.
        /// Example: "UPOW"
        /// </summary>
        public string DisplaySymbol { get; set; } = string.Empty;

        /// <summary>
        /// The Financial Instrument Global Identifier (FIGI).
        /// Example: "BBG000BGHYF2"
        /// </summary>
        public string Figi { get; set; } = string.Empty;

        /// <summary>
        /// The Market Identifier Code (MIC).
        /// Example: "OTCM"
        /// </summary>
        public string Mic { get; set; } = string.Empty;

        /// <summary>
        /// The actual stock symbol/ticker.
        /// Example: "UPOW"
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// The type of security.
        /// Example: "Common Stock"
        /// </summary>
        public string Type { get; set; } = string.Empty;
    }
}
