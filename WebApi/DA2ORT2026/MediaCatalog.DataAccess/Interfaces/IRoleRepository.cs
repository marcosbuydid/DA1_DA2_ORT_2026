using MediaCatalog.Domain;

namespace MediaCatalog.DataAccess.Interfaces
{
    public interface IRoleRepository
    {
        List<Role> GetRoles();
        Role? GetRole(Func<Role, bool> filter);
        void AddRole(Role role);
        void DeleteRole(Role role);
        bool Exists(Func<Role, bool> predicate);
    }
}
