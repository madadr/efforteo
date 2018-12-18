namespace Efforteo.Common.Auth
{
    public class JsonWebToken
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public long Expires { get; set; }
    }
}