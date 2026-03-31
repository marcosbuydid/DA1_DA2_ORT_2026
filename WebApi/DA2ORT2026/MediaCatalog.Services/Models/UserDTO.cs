
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class UserDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [RegularExpression("^[{1,2}]$", ErrorMessage = "Role id 1 = Administrator, Role id 2 = User")]
        public int RoleId { get; set; }

        public UserDTO()
        {
        }

        public UserDTO(int? id, string name, string lastName, string email, string password, int roleId)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            Password = password;
            RoleId = roleId;
        }

        public UserDTO(int? id, string name, string lastName, string email, int roleId)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Email = email;
            RoleId = roleId;
        }
    }
}
