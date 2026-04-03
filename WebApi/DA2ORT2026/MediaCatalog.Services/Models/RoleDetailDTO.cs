
namespace MediaCatalog.Services.Models
{
    public class RoleDetailDTO
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public RoleDetailDTO(){}

        public RoleDetailDTO(int? id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
