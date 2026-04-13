using MediaCatalog.Domain;
using MediaCatalog.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MediaCatalog.DataAccess.EFRepositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public EFUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User? GetUser(Func<User, bool> filter)
        {
            return _context.Set<User>().FirstOrDefault(filter);
        }

        public List<User> GetUsers()
        {
            return _context.Set<User>().AsQueryable<User>().ToList();
        }

        public void AddUser(User user)
        {
            _context.Set<User>().Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            _context.Set<User>().Remove(user);
            _context.SaveChanges();
        }

        public bool Exists(Expression<Func<User, bool>> predicate)
        {
            return _context.Users.Any(predicate);
        }
    }
}
