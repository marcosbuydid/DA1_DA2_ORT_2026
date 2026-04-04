
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public RoleDetailDTO AddRole(RoleCreateDTO role)
        {
            ValidateUniqueName(role.Name);
            _roleRepository.AddRole(ToEntity(role));
            return new RoleDetailDTO() { Name = role.Name };
        }

        public void DeleteRole(string name)
        {
            Role? roleToDelete = _roleRepository.GetRole(r => r.Name == name);
            if (roleToDelete == null)
            {
                throw new ServiceException("Cannot find a role with this name");
            }

            _roleRepository.DeleteRole(roleToDelete);
        }

        public void DeleteRoleById(int? roleId)
        {
            Role? roleToDelete = _roleRepository.GetRole(r => r.Id == roleId);
            if (roleToDelete == null)
            {
                throw new ServiceException("Cannot find a role with this name");
            }

            _roleRepository.DeleteRole(roleToDelete);
        }

        public RoleDetailDTO GetRole(string name)
        {
            Role? role = _roleRepository.GetRole(role => role.Name == name);
            if (role == null)
            {
                throw new ServiceException("Cannot find a role with this name");
            }

            return FromEntity(role);
        }

        public List<RoleDetailDTO> GetRoles()
        {
            List<RoleDetailDTO> rolesDTO = new List<RoleDetailDTO>();

            foreach (var role in _roleRepository.GetRoles())
            {
                rolesDTO.Add(FromEntity(role));
            }

            return rolesDTO;
        }

        private void ValidateUniqueName(string name)
        {
            string inputName = name.Trim().ToLowerInvariant();
            foreach (var role in _roleRepository.GetRoles())
            {
                string retrievedName = role.Name.Trim().ToLowerInvariant();
                if (retrievedName == inputName)
                {
                    throw new ServiceException("There`s a role already defined with that name");
                }
            }
        }

        private static Role ToEntity(RoleCreateDTO roleDTO)
        {
            return new Role() { Name = roleDTO.Name };
        }

        private static RoleDetailDTO FromEntity(Role role)
        {
            return new RoleDetailDTO(role.Id,role.Name);
        }
    }
}
