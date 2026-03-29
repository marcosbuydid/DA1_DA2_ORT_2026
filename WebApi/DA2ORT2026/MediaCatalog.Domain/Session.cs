
using MediaCatalog.Domain.Exceptions;

namespace MediaCatalog.Domain
{
    public class Session
    {
        private int _id;
        private string _token;
        private User _user;
        private DateTime _createdAt;

        public int Id 
        { 
            get => _id; 
            set => _id = value;
        }

        public string Token
        {
            get => _token;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new DomainException("Token cannot be null or empty");
                }
                _token = value;
            }
        }

        public User User
        {
            get => _user;
            set 
            {
                if (value == null)
                {
                    throw new DomainException("User cannot be null");
                }
                _user = value;
            } 
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

        public Session() { }

        public Session(int id, string token, User user) 
        { 
            Id = id;
            Token = token;
            User = user;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
