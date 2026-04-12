
using MediaCatalog.Domain;

namespace MediaCatalog.DataAccess.Interfaces
{
    public interface ISessionRepository
    {
        void AddSession(Session session);
        Session? GetSession(Func<Session, bool> filter);
    }
}
