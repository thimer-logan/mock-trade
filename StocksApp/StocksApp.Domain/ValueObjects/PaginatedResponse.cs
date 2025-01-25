namespace StocksApp.Domain.ValueObjects
{
    public class PaginatedResponse<T>
    {
        public int Count { get; set; }

        public IEnumerable<T> Result { get; set; }
    }
}
