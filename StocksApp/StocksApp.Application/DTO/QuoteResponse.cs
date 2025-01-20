namespace StocksApp.Application.DTO
{
    public class QuoteResponse
    {
        public float c { get; set; }  // Current price
        public float h { get; set; }  // High price of the day
        public float l { get; set; }  // Low price of the day
        public float o { get; set; }  // Open price of the day
        public float pc { get; set; } // Previous close price
        public long t { get; set; }   // Timestamp
    }
}
