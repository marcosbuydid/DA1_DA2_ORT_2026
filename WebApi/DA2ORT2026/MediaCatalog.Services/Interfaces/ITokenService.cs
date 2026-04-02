
using System.Text.Json;

namespace MediaCatalog.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string name, string email, string secretKey);
        bool ValidateToken(string token, string secretKey, out JsonElement payload);
    }
}
