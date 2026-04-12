
using MediaCatalog.Domain;
using System.Linq.Expressions;

namespace MediaCatalog.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User? GetUser(Func<User, bool> filter);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
        bool Exists(Expression<Func<User, bool>> predicate);
    }
}
