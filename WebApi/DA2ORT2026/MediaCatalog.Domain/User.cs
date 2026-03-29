
using MediaCatalog.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace MediaCatalog.Domain
{
    public class User
    {
        private int _id;
        private string _name;
        private string _lastname;
        private string _email;
        private string _password;
        private Role _role;

        public int Id
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
                _name = value;
            }
        }

        public string LastName
        {
            get => _lastname;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new DomainException("LastName cannot be null or empty");
                }
                _lastname = value;
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new DomainException("Email cannot be null or empty");
                }

                string validEmailPattern = @"^(?=.{1,254}$)(?=.{1,64}@)" +
                    @"[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+" + @"(\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*" +
                    @"@" + @"(" + @"([A-Za-z0-9]" +
                    @"([A-Za-z0-9-]{0,61}[A-Za-z0-9])?\.)+" + @"[A-Za-z]{2,}" + @"|" +
                    @"\[(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])" +
                    @"(\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])){3}\]" + @")$";

                if (!Regex.IsMatch(value, validEmailPattern))
                {
                    throw new DomainException("Email format is invalid");
                }

                _email = value;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new DomainException("Password cannot be null or empty");
                }

                if (value.Length < 8)
                {
                    throw new DomainException("Password must have eight chars or more");
                }

                _password = value;
            }
        }

        public Role Role
        {
            get => _role;
            set => _role = value;
        }

        public User() { }

        public User(int id, string name, string lastname, string email, string password, Role role)
        {
            Id = id;
            Name = name;
            LastName = lastname;
            Email = email;
            Password = password;
            Role = role;
        }

    }
}
