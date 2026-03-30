
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services.Interfaces
{
    public interface IRoleService
    {
        List<RoleDTO> GetRoles();
        RoleDTO GetRole(string name);
        void AddRole(RoleDTO role);
        void DeleteRole(string name);
    }
}
