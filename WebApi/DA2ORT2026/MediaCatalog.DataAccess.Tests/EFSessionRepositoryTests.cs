
using MediaCatalog.DataAccess.EFRepositories;
using MediaCatalog.Domain;
using MediaCatalog.Services.Interfaces.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MediaCatalog.DataAccess.Tests
{
    [TestClass]
    public class EFSessionRepositoryTests
    {
        private ISessionRepository _sessionRepository;
        private AppDbContext _appDbContext;
        private SqliteConnection _connection;
        private Role _role;
        private User _user;

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

            _sessionRepository = new EFSessionRepository(_appDbContext);

            _role = new Role(1, "Administrator");
            _user = new User(1, "aName", "aLastName", "test@mail.com", "aPassword!.", 1);
            Session session = new Session(1, "auth_token,", _user);

            List<Role> roles = new List<Role> { _role };
            List<User> users = new List<User> { _user };
            List<Session> sessions = new List<Session> { session };

            _appDbContext.Roles.AddRange(roles);
            _appDbContext.SaveChanges();

            _appDbContext.Users.AddRange(users);
            _appDbContext.SaveChanges();

            _appDbContext.Sessions.AddRange(sessions);
            _appDbContext.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _appDbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetSession_WhenCalled_ThenSessionIsReturned()
        {
            //arrange
            Session expectedSession = _appDbContext.Sessions.First();

            //act
            Session? retrievedSession = _sessionRepository.GetSession(s => s.Token == expectedSession.Token);

            //assert
            Assert.AreEqual(expectedSession, retrievedSession);
        }

        [TestMethod]
        public void AddSession_WhenCalled_ThenSessionIsAdded()
        {
            //arrange
            Session newSession = new Session(2, "new_auth_token", _user);

            //act
            _sessionRepository.AddSession(newSession);

            //assert
            Session? retrievedSession = _sessionRepository.GetSession(s => s.Token == newSession.Token);
            Assert.IsNotNull(retrievedSession);
            Assert.AreEqual(newSession, retrievedSession);
        }
    }
}
