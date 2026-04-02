
using MediaCatalog.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MediaCatalog.Services
{
    public class JWTService : ITokenService
    {
        public string GenerateToken(string name, string email, string secretKey)
        {
            var header = new
            {
                alg = "HS256",
                typ = "JWT"
            };

            var payload = new
            {
                name = name,
                email = email,
                exp = DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds()
            };

            string headerJson = JsonSerializer.Serialize(header);
            string payloadJson = JsonSerializer.Serialize(payload);

            string headerEncoded = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
            string payloadEncoded = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));

            string unsignedToken = $"{headerEncoded}.{payloadEncoded}";

            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            using var hmac = new HMACSHA256(keyBytes);
            byte[] signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));

            string signature = Base64UrlEncode(signatureBytes);

            return $"{unsignedToken}.{signature}";
        }

        public bool ValidateToken(string token, string secretKey, out JsonElement payload)
        {
            payload = default;

            //1-split token
            var parts = token.Split('.');
            if (parts.Length != 3)
                return false;

            string headerEncoded = parts[0];
            string payloadEncoded = parts[1];
            string signatureProvided = parts[2];

            //2-recompute signature
            string unsignedToken = $"{headerEncoded}.{payloadEncoded}";
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            using var hmac = new HMACSHA256(keyBytes);
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));
            string computedSignature = Base64UrlEncode(computedHash);

            //3-compare signatures (using constant-time comparison)
            if (!CryptographicOperations.FixedTimeEquals(
                    Encoding.UTF8.GetBytes(signatureProvided),
                    Encoding.UTF8.GetBytes(computedSignature)))
            {
                return false;
            }

            //4-decode payload
            byte[] payloadBytes = Base64UrlDecode(payloadEncoded);
            payload = JsonSerializer.Deserialize<JsonElement>(payloadBytes);

            //5-check expiration
            if (!payload.TryGetProperty("exp", out JsonElement expElement))
                return false;

            long exp = expElement.GetInt64();
            long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            if (now >= exp)
                return false;

            return true;
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private static byte[] Base64UrlDecode(string input)
        {
            string padded = input
                .Replace('-', '+')
                .Replace('_', '/');

            switch (padded.Length % 4)
            {
                case 2: padded += "=="; break;
                case 3: padded += "="; break;
            }

            return Convert.FromBase64String(padded);
        }
    }
}
