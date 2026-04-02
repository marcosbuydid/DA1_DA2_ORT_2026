
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;

namespace MediaCatalog.DataAccess.InMemoryRepositories
{
    public class InMemorySessionRepository : ISessionRepository
    {
        private List<Session> Sessions { get; }

        public InMemorySessionRepository()
        {
            Sessions = new List<Session>();
        }

        public void AddSession(Session session)
        {
            Sessions.Add(session);
        }

        public Session? GetSession(Func<Session, bool> filter)
        {
            return Sessions.Where(filter).FirstOrDefault();
        }

        public bool Exists(Func<Session, bool> predicate)
        {
            return Sessions.Where(predicate).Any();
        }
    }
}
