
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services.Interfaces
{
    public interface IUserService
    {
        List<UserDTO> GetUsers();
        UserDTO GetUser(string email);
        UserDTO AddUser(UserDTO user);
        void DeleteUser(string email);
        UserDTO UpdateUser(UserDTO user);
        void ChangePassword(ChangePasswordDTO changePasswordDTO);
    }
}
