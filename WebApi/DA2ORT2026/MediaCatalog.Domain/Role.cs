
using MediaCatalog.Domain.Exceptions;

namespace MediaCatalog.Domain
{
    public class Role
    {
        private int? _id;
        private string _name;

        private static readonly HashSet<string>
            Roles = new HashSet<string>() { "Administrator", "User" };

        public int? Id
        {
            get => _id;
            set => _id = value;
        }

        public string Name
        {
            get => _name;

            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new DomainException("Name cannot be null or empty");
                }

                if (!Roles.Contains(value))
                    throw new DomainException("Invalid role");

                _name = value;
            }
        }

        public Role() { }

        public Role(int? id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
