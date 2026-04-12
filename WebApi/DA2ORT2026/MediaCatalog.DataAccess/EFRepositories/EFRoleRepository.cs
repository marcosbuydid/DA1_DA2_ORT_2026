
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using System.Linq.Expressions;

namespace MediaCatalog.DataAccess.EFRepositories
{
    public class EFRoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public EFRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public Role? GetRole(Func<Role, bool> filter)
        {
            return _context.Set<Role>().FirstOrDefault(filter);
        }

        public List<Role> GetRoles()
        {
            return _context.Set<Role>().AsQueryable<Role>().ToList();
        }

        public void AddRole(Role role)
        {
            _context.Set<Role>().Add(role);
            _context.SaveChanges();
        }

        public void DeleteRole(Role role)
        {
            _context.Set<Role>().Remove(role);
            _context.SaveChanges();
        }

        public bool Exists(Expression<Func<Role, bool>> predicate)
        {
            return _context.Roles.Any(predicate);
        }
    }
}
