using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISecureDataService _secureDataService;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository,
            ISecureDataService secureDataService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _secureDataService = secureDataService;
        }

        public UserDetailDTO AddUser(UserCreateDTO user)
        {
            ValidateUserEmail(user.Email);
            Role? role = _roleRepository.GetRole(r => r.Id == user.RoleId);

            if (role == null)
            {
                throw new ServiceException("Role is invalid");
            }

            user.Password = _secureDataService.Hash(user.Password);
            _userRepository.AddUser(ToEntity(user));

            return new UserDetailDTO()
            {
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                RoleId = (int)role.Id
            };
        }

        public List<UserDetailDTO> GetUsers()
        {
            List<UserDetailDTO> usersDTO = new List<UserDetailDTO>();

            foreach (var user in _userRepository.GetUsers())
            {
                usersDTO.Add(FromEntity(user));
            }

            return usersDTO;
        }

        public void DeleteUser(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new ServiceException("Email cannot be null or empty");
            }

            User? userToDelete = _userRepository.GetUser(u => u.Email.Equals(email,
                StringComparison.OrdinalIgnoreCase));

            if (userToDelete == null)
            {
                throw new ResourceNotFoundException("Cannot find a user with this email");
            }

            _userRepository.DeleteUser(userToDelete);
        }

        public void DeleteUserById(int? userId)
        {
            User? userToDelete = _userRepository.GetUser(u => u.Id == userId);

            if (userToDelete == null)
            {
                throw new ResourceNotFoundException("Cannot find a user with this id");
            }

            _userRepository.DeleteUser(userToDelete);
        }


        public UserDetailDTO UpdateUserById(int? userId, UserUpdateDTO userToUpdate)
        {
            User? user = _userRepository.GetUser(u => u.Id == userId);

            if (user == null)
            {
                throw new ResourceNotFoundException("Cannot find the specified user with this id");
            }

            Role? userToUpdateRole = _roleRepository.GetRole(r => r.Id == userToUpdate.RoleId);

            if (userToUpdateRole == null)
            {
                throw new ServiceException("Role is invalid");
            }

            user.Name = userToUpdate.Name;
            user.LastName = userToUpdate.LastName;
            user.Role = userToUpdateRole;
            _userRepository.UpdateUser(user);

            return FromEntity(user);
        }

        public UserDetailDTO UpdateUser(string? email, UserUpdateDTO userToUpdate)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new ServiceException("Email cannot be null or empty");
            }

            User? user = _userRepository.GetUser(u => u.Email.Equals(email,
                StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                throw new ResourceNotFoundException("Cannot find the specified user");
            }

            Role? userToUpdateRole = _roleRepository.GetRole(r => r.Id == userToUpdate.RoleId);

            if (userToUpdateRole == null)
            {
                throw new ServiceException("Role is invalid");
            }

            user.Name = userToUpdate.Name;
            user.LastName = userToUpdate.LastName;
            user.Role = userToUpdateRole;
            _userRepository.UpdateUser(user);

            return FromEntity(user);
        }

        public UserDetailDTO GetUser(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new ServiceException("Email cannot be null or empty");
            }

            User? user = _userRepository.GetUser(u => u.Email.Equals(email,
                StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                throw new ResourceNotFoundException("Cannot find a user with this email");
            }

            return FromEntity(user);
        }

        public void ChangePassword(string? email, ChangePasswordDTO changePasswordDTO)
        {
            User? user = ValidateOldPassword(email, changePasswordDTO.OldPassword);

            if (user != null)
            {
                string newPasswordHash = _secureDataService.Hash(changePasswordDTO.NewPassword);
                user.Password = newPasswordHash;
                _userRepository.UpdateUser(user);
            }
            else
            {
                throw new ServiceException("Old password entered is incorrect");
            }
        }

        private void ValidateUserEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new ServiceException("Email cannot be null or empty");
            }

            string inputEmail = email.Trim().ToLowerInvariant();

            //hash set for faster lookups if repository has many items
            HashSet<string> userEmails = new HashSet<string>(
                _userRepository.GetUsers()
                    .Select(u => u.Email.ToLowerInvariant()));

            if (userEmails.Contains(inputEmail))
            {
                throw new ConflictException("There's a user already defined with that email");
            }
        }

        private User? ValidateOldPassword(string? email, string inputPassword)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                throw new ServiceException("Email cannot be null or empty");
            }

            User? user = _userRepository.GetUser(u => u.Email.Equals(email,
                StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                throw new ResourceNotFoundException("Cannot find user with this email");
            }

            string storedHashedPassword = user.Password;
            bool hashesMatch = _secureDataService.CompareHashes(storedHashedPassword, inputPassword);

            return hashesMatch ? user : null;
        }

        private static User ToEntity(UserCreateDTO userDTO)
        {
            Role role = new Role() { Id = userDTO.RoleId };
            return new User()
            {
                Name = userDTO.Name,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Password = userDTO.Password,
                Role = role
            };
        }

        private static UserDetailDTO FromEntity(User user)
        {
            return new UserDetailDTO()
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                RoleId = (int)user.Role.Id,
            };
        }
    }
}
