namespace StocksApp.Application.DTO.Authentication
{
    public class AuthenticationResponse
    {
        public string? FirstName { get; set; } = string.Empty;

        public string? LastName { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;

        public string? Token { get; set; } = string.Empty;

        public string? RefreshToken { get; set; } = string.Empty;

        public DateTime Expiration { get; set; }

        public DateTime RefreshExpiration { get; set; }
    }
}
