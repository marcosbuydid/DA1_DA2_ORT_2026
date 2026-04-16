
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services.Interfaces
{
    public interface IUserService
    {
        List<UserDetailDTO> GetUsers();
        UserDetailDTO GetUser(string email);
        UserDetailDTO AddUser(UserCreateDTO user);
        void DeleteUser(string email);
        void DeleteUserById(int userId);
        UserDetailDTO UpdateUser(string email, UserUpdateDTO user);
        UserDetailDTO UpdateUserById(int userId, UserUpdateDTO user);
        void ChangePassword(string email, ChangePasswordDTO changePasswordDTO);
    }
}
