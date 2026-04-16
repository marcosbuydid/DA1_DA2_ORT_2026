using MediaCatalog.DataAccess.EFRepositories;
using MediaCatalog.Domain;
using MediaCatalog.Services.Interfaces.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MediaCatalog.DataAccess.Tests
{
    [TestClass]
    public class EFUserRepositoryTests
    {
        private IUserRepository _userRepository;
        private AppDbContext _appDbContext;
        private SqliteConnection _connection;
        private Role _role;
        private User _user;
        private List<Role> _roles;
        private List<User> _users;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            _appDbContext = new AppDbContext(options);
            _appDbContext.Database.EnsureCreated();

            _userRepository = new EFUserRepository(_appDbContext);

            _role = new Role(2, "User");
            _user = new User(1, "aName", "aLastName", "mail@test.com", "password12!!", (int)_role.Id);

            _roles = new List<Role> { _role };
            _users = new List<User> { _user };

            _appDbContext.Roles.AddRange(_roles);
            _appDbContext.Users.AddRange(_users);

            _appDbContext.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _appDbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetUsers_WhenCalled_ThenAllUsersAreReturned()
        {
            //arrange
            List<User> expectedUsers = _appDbContext.Users.ToList();

            //act
            List<User> retrievedUsers = _userRepository.GetUsers();

            //assert
            CollectionAssert.AreEqual(expectedUsers, retrievedUsers);
        }

        [TestMethod]
        public void GetUser_WhenCalled_ThenUserIsReturned()
        {
            //arrange
            User expectedUser = _appDbContext.Users.First();

            //act
            User? retrievedUser = _userRepository.GetUser(u => u.Id == expectedUser.Id);

            //assert
            Assert.AreEqual(expectedUser, retrievedUser);
        }

        [TestMethod]
        public void AddUser_WhenCalled_ThenUserIsAdded()
        {
            //arrange
            User newUser = new User(2, "aName", "aLastName", "email@test.com", "password452!!", (int)_role.Id);

            //act
            _userRepository.AddUser(newUser);

            //assert
            User? retrievedUser = _userRepository.GetUser(u => u.Id == newUser.Id);
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(newUser, retrievedUser);
        }

        [TestMethod]
        public void UpdateUser_WhenCalled_ThenUserIsUpdated()
        {
            //assert
            User existentUser = _appDbContext.Users.First();
            existentUser.Name = "Updated Name";
            existentUser.LastName = "Updated LastName";

            //act
            _userRepository.UpdateUser(existentUser);

            //assert
            User? retrievedUser = _userRepository.GetUser(u => u.Id == existentUser.Id);
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(existentUser, retrievedUser);
        }

        [TestMethod]
        public void DeleteUser_WhenCalled_ThenUserIsDeleted()
        {
            //arrange
            User existingUser = _appDbContext.Users.First();

            //act
            _userRepository.DeleteUser(existingUser);

            //assert
            User? retrievedUser = _userRepository.GetUser(u => u.Id == existingUser.Id);
            Assert.IsNull(retrievedUser);
        }

        [TestMethod]
        public void Exists_WhenCalledWithExistentEntity_ThenReturnTrue()
        {
            //arrange
            User newUser = new User(3, "aName", "aLastName", "mail@test.com", "password87-4!!", (int)_role.Id);
            _appDbContext.Users.Add(newUser);
            _appDbContext.SaveChanges();

            //act
            bool exists = _userRepository.Exists(predicate: u => u.Email == "mail@test.com");

            //assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void Exists_WhenCalledWithNonExistentEntity_ThenReturnFalse()
        {
            //arrange

            //act
            bool exists = _userRepository.Exists(predicate: u => u.Email == "test@mail.com");

            //assert
            Assert.IsFalse(exists);
        }
    }
}
