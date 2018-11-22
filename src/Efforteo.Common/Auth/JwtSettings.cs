namespace Efforteo.Common.Auth
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public int ExpiryMinutes { get; set; }
        public string Issuer { get; set; }
    }
}