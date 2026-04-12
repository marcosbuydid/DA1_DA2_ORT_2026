
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;

namespace MediaCatalog.DataAccess.EFRepositories
{
    public class EFSessionRepository : ISessionRepository
    {
        private readonly AppDbContext _context;

        public EFSessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AddSession(Session session)
        {
            _context.Set<Session>().Add(session);
            _context.SaveChanges();
        }

        public Session? GetSession(Func<Session, bool> filter)
        {
            return _context.Set<Session>().FirstOrDefault(filter);
        }
    }
}
