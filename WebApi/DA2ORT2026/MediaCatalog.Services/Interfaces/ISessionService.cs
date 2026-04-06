
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services.Interfaces
{
    public interface ISessionService
    {
        string Authenticate(string username, string password);
        SessionDTO? ValidateSession(string token);
        void SignOut();
    }
}
