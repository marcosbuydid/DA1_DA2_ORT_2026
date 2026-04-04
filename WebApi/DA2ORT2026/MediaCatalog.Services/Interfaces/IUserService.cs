
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services.Interfaces
{
    public interface IUserService
    {
        List<UserDetailDTO> GetUsers();
        UserDetailDTO GetUser(string email);
        UserDetailDTO AddUser(UserCreateDTO user);
        void DeleteUser(string email);
        UserDetailDTO UpdateUser(UserUpdateDTO user);
        void ChangePassword(ChangePasswordDTO changePasswordDTO);
    }
}
