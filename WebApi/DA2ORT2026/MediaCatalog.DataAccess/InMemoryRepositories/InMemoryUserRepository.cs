using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using System.Data;
using System.Linq.Expressions;

namespace MediaCatalog.DataAccess.InMemoryRepositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private List<User> Users { get; }

        public InMemoryUserRepository()
        {
            Users = new List<User>();
            LoadDefaultUsers();
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

        public bool Exists(Expression<Func<User, bool>> predicate)
        {
            return Users.AsQueryable().Any(predicate);
        }

        private void LoadDefaultUsers()
        {
            Users.Add(new User(1, "Marcos", "Buydid", "marcosb@email.com", 
                "YfCcXFdr5hMSfeP2PqGnLahaL/Aq7qDX78vZTnxYlB3iC6FQHcQi5AB9ETjAWY66", 
                new Role(1, "Administrator")));
        }
    }
}
