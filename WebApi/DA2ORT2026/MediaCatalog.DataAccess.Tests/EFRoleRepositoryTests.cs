
using MediaCatalog.DataAccess.EFRepositories;
using MediaCatalog.Domain;
using MediaCatalog.Services.Interfaces.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;


namespace MediaCatalog.DataAccess.Tests
{
    [TestClass]
    public class EFRoleRepositoryTests
    {
        private IRoleRepository _roleRepository;
        private AppDbContext _appDbContext;
        private SqliteConnection _connection;

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

            _roleRepository = new EFRoleRepository(_appDbContext);

            Role role1 = new Role(1, "Administrator");
            List<Role> roles = new List<Role> { role1 };

            _appDbContext.Roles.AddRange(roles);
            _appDbContext.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _appDbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetRoles_WhenCalled_ThenAllRolesAreReturned()
        {
            //arrange
            List<Role> expectedRoles = _appDbContext.Roles.ToList();

            //act
            List<Role> retrievedRoles = _roleRepository.GetRoles();

            //assert
            CollectionAssert.AreEqual(expectedRoles, retrievedRoles);
        }

        [TestMethod]
        public void GetRole_WhenCalled_ThenRoleIsReturned()
        {
            //arrange
            Role expectedRole = _appDbContext.Roles.First();

            //act
            Role? retrievedRole = _roleRepository.GetRole(r => r.Id == expectedRole.Id);

            //assert
            Assert.AreEqual(expectedRole, retrievedRole);
        }

        [TestMethod]
        public void AddRole_WhenCalled_ThenRoleIsAdded()
        {
            //arrange
            Role newRole = new Role(2, "User");

            //act
            _roleRepository.AddRole(newRole);

            //assert
            Role? retrievedRole = _roleRepository.GetRole(r => r.Id == newRole.Id);
            Assert.IsNotNull(retrievedRole);
            Assert.AreEqual(newRole, retrievedRole);
        }

        [TestMethod]
        public void DeleteRole_WhenCalled_ThenRoleIsDeleted()
        {
            //arrange
            Role existingRole = _appDbContext.Roles.First();

            //act
            _roleRepository.DeleteRole(existingRole);

            //assert
            Role? retrievedRole = _roleRepository.GetRole(r => r.Id == existingRole.Id);
            Assert.IsNull(retrievedRole);
        }

        [TestMethod]
        public void Exists_WhenCalledWithExistentEntity_ThenReturnTrue()
        {
            //arrange
            Role role = new Role(2, "User");
            _appDbContext.Roles.Add(role);
            _appDbContext.SaveChanges();

            //act
            bool exists = _roleRepository.Exists(predicate: r => r.Name == "User");

            //assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void Exists_WhenCalledWithNonExistentEntity_ThenReturnFalse()
        {
            //arrange

            //act
            bool exists = _roleRepository.Exists(predicate: r => r.Name == "aRole");

            //assert
            Assert.IsFalse(exists);
        }
    }
}
