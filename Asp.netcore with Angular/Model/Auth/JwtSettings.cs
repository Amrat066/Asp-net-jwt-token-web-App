namespace Asp.netcore_with_Angular.Model.Auth
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresMinutes { get; set; }
    }
}
