
using MediaCatalog.Domain;

namespace MediaCatalog.Services.Interfaces.Repositories
{
    public interface ISessionRepository
    {
        void AddSession(Session session);
        Session? GetSession(Func<Session, bool> filter);
    }
}
