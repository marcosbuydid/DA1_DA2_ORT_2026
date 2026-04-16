
using System.Text.Json;

namespace MediaCatalog.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string name, string email, string secretKey, int tokenExpMinutes);

        bool ValidateToken(string token, string secretKey, string expectedIssuer, 
            string expectedAudience, out JsonElement payload);
    }
}
