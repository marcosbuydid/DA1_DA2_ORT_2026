
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role id is required.")]
        public int RoleId { get; set; }

        public UserCreateDTO()
        {
        }

        public UserCreateDTO(string name, string lastName, string email, string password, int roleId)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            Password = password;
            RoleId = roleId;
        }

        public UserCreateDTO(string name, string lastName, string email, int roleId)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            RoleId = roleId;
        }
    }
}
