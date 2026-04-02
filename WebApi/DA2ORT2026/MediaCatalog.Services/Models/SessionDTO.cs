
namespace MediaCatalog.Services.Models
{
    public class SessionDTO
    {
        public int? Id { get; set; }
        public string? Token { get; set; }
        public UserDTO? LoggedUser { get; set; }
        public string? LoggedUserRoleName { get; set; }

        public SessionDTO() { }

        public SessionDTO(int? id, string token, UserDTO loggedUser, string loggedUserRoleName)
        {
          Id = id;
          Token = token;
          LoggedUser = loggedUser;
          LoggedUserRoleName = loggedUserRoleName;
        }
    }
}
