using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using System.Data;

namespace MediaCatalog.DataAccess.InMemoryRepositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private List<User> Users { get; }

        public InMemoryUserRepository()
        {
            Users = new List<User>();
        }

        public List<User> GetUsers()
        {
            return Users;
        }

        public User? GetUser(Func<User, bool> filter)
        {
            return Users.Where(filter).FirstOrDefault();
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public void UpdateUser(User userToUpdate)
        {
            User? user = Users.Find(u => u.Email == userToUpdate.Email);
            var userToUpdateIndex = Users.IndexOf(user);
            userToUpdate.Password = user.Password;
            Users[userToUpdateIndex] = userToUpdate;
        }

        public void DeleteUser(User user)
        {
            Users.Remove(user);
        }

        public bool Exists(Func<User, bool> predicate)
        {
            return Users.Where(predicate).Any();
        }
    }
}
