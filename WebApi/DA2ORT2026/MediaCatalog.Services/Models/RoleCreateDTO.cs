
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class RoleCreateDTO
    {
        [RegularExpression("^(Administrator|User)$",
            ErrorMessage = "Role must be Administrator or User")]
        public string Name { get; set; }

        public RoleCreateDTO() { }

        public RoleCreateDTO(string name)
        {
            Name = name;
        }
    }
}
