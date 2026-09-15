
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class RoleCreateDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public RoleCreateDTO() { }

        public RoleCreateDTO(string name)
        {
            Name = name;
        }
    }
}
