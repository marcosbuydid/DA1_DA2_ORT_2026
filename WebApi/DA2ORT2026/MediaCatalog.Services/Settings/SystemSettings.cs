
namespace MediaCatalog.Services.Settings
{
    public class SystemSettings
    {
        public string Token { get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenExpMinutes { get; set; }
    }
}
