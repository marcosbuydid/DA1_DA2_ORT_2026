
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using MediaCatalog.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MediaCatalog.Services
{
    public class JWTService : ITokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public JWTService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public string GenerateToken(string name, string email, string secretKey, int tokenExpMinutes)
        {
            //user is validated previously on ValidateUserCredentials, no need to check if is null
            User? user = _userRepository.GetUsers().FirstOrDefault(u => u.Email == email);

            Role? userRole = _roleRepository.GetRoles().FirstOrDefault(r => r.Id == user.RoleId);

            var header = new
            {
                alg = "HS256",
                typ = "JWT"
            };

            var payload = new
            {
                name = user.Name,
                lastName = user.LastName,
                email = user.Email,
                roleId = user.RoleId,
                roleName = userRole.Name,
                iss = "MediaCatalogAPI", //token issuer
                aud = "MediaCatalogWebApp", //who can consume the token
                exp = DateTimeOffset.UtcNow.AddMinutes(tokenExpMinutes).ToUnixTimeSeconds()
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

        public bool ValidateToken(string token, string secretKey, string expectedIssuer, 
            string expectedAudience, out JsonElement payload)
        {
            payload = default;

            if(token == null)
                return false;

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

            //5-check issuer
            if (!payload.TryGetProperty("iss", out JsonElement issElement))
                return false;

            string? issuer = issElement.GetString();

            if (issuer != expectedIssuer)
                return false;

            //6-check audience
            if (!payload.TryGetProperty("aud", out JsonElement audElement))
                return false;

            string? audience = audElement.GetString();

            if (audience != expectedAudience)
                return false;

            //7-check expiration
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
            return Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        private static byte[] Base64UrlDecode(string input)
        {
            string padded = input.Replace('-', '+').Replace('_', '/');

            switch (padded.Length % 4)
            {
                case 2: padded += "=="; break;
                case 3: padded += "="; break;
            }

            return Convert.FromBase64String(padded);
        }
    }
}
