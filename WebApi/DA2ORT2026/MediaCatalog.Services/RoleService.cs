
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
        private readonly IUserRepository _userRepository;

        public RoleService(IRoleRepository roleRepository, 
            IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public RoleDetailDTO AddRole(RoleCreateDTO role)
        {
            ValidateUniqueName(role.Name);
            _roleRepository.AddRole(ToEntity(role));
            return new RoleDetailDTO() { Name = role.Name };
        }

        public void DeleteRole(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ServiceException("Name cannot be null or empty");
            }

            Role? roleToDelete = _roleRepository.GetRole(r => r.Name.Equals(name,
                StringComparison.OrdinalIgnoreCase));

            if (roleToDelete == null)
            {
                throw new ResourceNotFoundException("Cannot find a role with this name");
            }

            //validate if role can be deleted
            if (RoleInUser(name))
            {
                throw new ServiceException("Role is being used by user");
            }

            _roleRepository.DeleteRole(roleToDelete);
        }

        public void DeleteRoleById(int? roleId)
        {
            Role? roleToDelete = _roleRepository.GetRole(r => r.Id == roleId);
            if (roleToDelete == null)
            {
                throw new ResourceNotFoundException("Cannot find a role with this id");
            }

            //validate if role can be deleted
            if (RoleInUser(roleToDelete.Name))
            {
                throw new ServiceException("Role is being used by user");
            }

            _roleRepository.DeleteRole(roleToDelete);
        }

        public RoleDetailDTO GetRole(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ServiceException("Name cannot be null or empty");
            }

            Role? role = _roleRepository.GetRole(r => r.Name.Equals(name,
                StringComparison.OrdinalIgnoreCase));

            if (role == null)
            {
                throw new ResourceNotFoundException("Cannot find a role with this name");
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
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ServiceException("Name cannot be null or empty");
            }

            string inputName = name.Trim().ToLowerInvariant();

            //hash set for faster lookups if repository has many items
            HashSet<string> roleNames = new HashSet<string>(_roleRepository.GetRoles()
                .Select(r => r.Name.ToLowerInvariant()));

            if (roleNames.Contains(inputName))
            {
                throw new ConflictException("There’s a role already defined with that name");
            }
        }

        private bool RoleInUser(string name)
        {   
            Role? role = _roleRepository.GetRole(r => r.Name.Equals(name,
                StringComparison.OrdinalIgnoreCase));

            return _userRepository.Exists(u => u.RoleId == role.Id);
        }

        private static Role ToEntity(RoleCreateDTO roleDTO)
        {
            return new Role() { Name = roleDTO.Name };
        }

        private static RoleDetailDTO FromEntity(Role role)
        {
            return new RoleDetailDTO(role.Id, role.Name);
        }
    }
}
