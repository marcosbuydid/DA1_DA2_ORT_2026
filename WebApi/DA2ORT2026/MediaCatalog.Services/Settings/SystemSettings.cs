
namespace MediaCatalog.Services.Settings
{
    public class SystemSettings
    {
        public string EncryptionKey { get; set; }
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenExpMinutes { get; set; }
    }
}
