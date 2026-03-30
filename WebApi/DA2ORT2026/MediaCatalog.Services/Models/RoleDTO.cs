
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class RoleDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public RoleDTO(){}

        public RoleDTO(int? id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
