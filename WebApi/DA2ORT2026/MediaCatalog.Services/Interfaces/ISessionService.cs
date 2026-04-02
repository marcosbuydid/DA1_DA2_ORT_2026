
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services.Interfaces
{
    public interface ISessionService
    {
        SessionDTO? ValidateSession(string token);
        void Authenticate(string username, string password);
        void SignOut();
    }
}
