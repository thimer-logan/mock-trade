namespace StocksApp.Domain.ValueObjects
{
    public class CompanyProfile
    {
        public string Country { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;

        /// <summary>
        /// The date of the company's initial public offering.
        /// Example: "1980-12-12"
        /// </summary>
        public DateTime? Ipo { get; set; }

        /// <summary>
        /// The market capitalization value (in millions, depending on the data provider).
        /// Example: 1415993
        /// </summary>
        public decimal MarketCapitalization { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Total number of shares outstanding.
        /// Example: 4375.47998046875
        /// </summary>
        public decimal ShareOutstanding { get; set; }

        public string Ticker { get; set; } = string.Empty;
        public string Weburl { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;

        /// <summary>
        /// Industry classification from Finnhub.
        /// Example: "Technology"
        /// </summary>
        public string FinnhubIndustry { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Name} ({Ticker})";
        }
    }
}
