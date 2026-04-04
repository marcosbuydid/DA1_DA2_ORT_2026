
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class UserDetailDTO
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int RoleId { get; set; }

        public UserDetailDTO()
        {
        }

        public UserDetailDTO(int? id, string name, string lastName, string email, string password, int roleId)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            Password = password;
            RoleId = roleId;
        }

        public UserDetailDTO(int? id, string name, string lastName, string email, int roleId)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            RoleId = roleId;
        }
    }
}
