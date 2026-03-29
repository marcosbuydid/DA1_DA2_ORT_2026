using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;

namespace MediaCatalog.DataAccess.InMemoryRepositories
{
    public class InMemoryRoleRepository : IRoleRepository
    {
        private List<Role> Roles { get; }

        public InMemoryRoleRepository()
        {
            Roles = new List<Role>();
        }

        public Role? GetRole(Func<Role, bool> filter)
        {
            return Roles.Where(filter).FirstOrDefault();
        }

        public List<Role> GetRoles()
        {
            return Roles;
        }

        public void AddRole(Role role)
        {
            Roles.Add(role);
        }

        public void DeleteRole(Role role)
        {
            Roles.Remove(role);
        }
    }
}
