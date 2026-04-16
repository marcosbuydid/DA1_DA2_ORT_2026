
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Role id is required.")]
        public int RoleId { get; set; }

        public UserUpdateDTO()
        {
        }

        public UserUpdateDTO(string name, string lastName, string email, int roleId)
        {
            Name = name;
            LastName = lastName;
            RoleId = roleId;
        }
    }
}
