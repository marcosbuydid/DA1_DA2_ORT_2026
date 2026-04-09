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
            LoadDefaultRoles();
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

        public bool Exists(Func<Role, bool> predicate)
        {
            return Roles.Where(predicate).Any();
        }

        private void LoadDefaultRoles()
        {
            Roles.Add(new Role(1, "Administrator"));
            Roles.Add(new Role(2, "User"));
        }
    }
}
