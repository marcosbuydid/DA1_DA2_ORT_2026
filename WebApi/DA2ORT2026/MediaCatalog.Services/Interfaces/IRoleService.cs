
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services.Interfaces
{
    public interface IRoleService
    {
        List<RoleDetailDTO> GetRoles();
        RoleDetailDTO GetRole(string name);
        RoleDetailDTO AddRole(RoleCreateDTO role);
        void DeleteRole(string name);
    }
}
