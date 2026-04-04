
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [RegularExpression("^[{1,2}]$", ErrorMessage = "Role id 1 = Administrator, Role id 2 = User")]
        public int RoleId { get; set; }

        public UserUpdateDTO()
        {
        }

        public UserUpdateDTO(string name, string lastName, string email, int roleId)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            RoleId = roleId;
        }
    }
}
