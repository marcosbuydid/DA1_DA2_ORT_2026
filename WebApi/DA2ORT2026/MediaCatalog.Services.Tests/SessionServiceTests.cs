
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using MediaCatalog.Services.Settings;
using Microsoft.Extensions.Options;
using Moq;
using System.Text.Json;

namespace MediaCatalog.Services.Tests
{
    [TestClass]
    public class SessionServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ISessionRepository> _sessionRepositoryMock;
        private Mock<ISecureDataService> _secureDataServiceMock;
        private Mock<ITokenService> _jwtServiceMock;
        private SessionService _sessionService;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
            _secureDataServiceMock = new Mock<ISecureDataService>(MockBehavior.Strict);

            var systemSettings = Options.Create(new SystemSettings
            {
                Token = "abcdefghijklmnopioBpLgpjWR2aHeotXSnsK1234567",
                SecretKey = "ab1c2a3b4c1a2b3c4a1b2c3a4b1c2a3b4c1a2b3c4a1b2="
            });

            _jwtServiceMock = new Mock<ITokenService>(MockBehavior.Strict);

            _sessionService = new SessionService(_sessionRepositoryMock.Object,
                _userRepositoryMock.Object, _secureDataServiceMock.Object, systemSettings,
                _jwtServiceMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _userRepositoryMock.VerifyAll();
            _sessionRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void Authenticate_WhenUsingValidCredentials_ThenSessionIsCreated()
        {
            //arrange
            Role role = new Role(1, "User");
            User user = new User(1, "John", "Doe", "john@test.com", "password123", role);

            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(new List<User> { user });
            
            //simulate passwords matches
            _secureDataServiceMock.Setup(s => s.CompareHashes(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            _sessionRepositoryMock.Setup(r => r.AddSession(It.IsAny<Session>()));

            _jwtServiceMock.Setup(t => t.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("fake-token");

            //act
            _sessionService.Authenticate("john@test.com", "password");

            //assert
            _sessionRepositoryMock.Verify(r => r.AddSession(It.IsAny<Session>()), Times.Once);
        }

        [TestMethod]
        public void Authenticate_WhenCredentialsAreInvalid_ThenThrowsException()
        {
            //arrange
            Role role = new Role(1, "User");
            User user = new User(1, "John", "Doe", "john@test.com", "password123", role);

            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(new List<User> { user });

            //password does not match
            _secureDataServiceMock.Setup(s => s.CompareHashes(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            //act
            Action action = () => _sessionService.Authenticate("john@mail.com", "wrongPassword");

            //assert
            Assert.ThrowsException<ServiceException>(action);

            _sessionRepositoryMock.Verify(r => r.AddSession(It.IsAny<Session>()),Times.Never);
        }

        [TestMethod]
        public void ValidateSession_WhenTokenIsValid_ThenSessionIsReturned()
        {
            //arrange
            SessionDTO session = new SessionDTO()
            {
                Token = "valid-token",
                LoggedUser = new UserDetailDTO { Name = "John" },
                LoggedUserRoleName = "User"
            };

            //simulate that user is already authenticated
            typeof(SessionService).
                GetField("_currentSession", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_sessionService, session);

            JsonElement fakePayload = default;

            _jwtServiceMock.Setup(s => s.ValidateToken(It.IsAny<string>(), It.IsAny<string>(), out fakePayload)).Returns(true);

            //act
            SessionDTO? sessionDTO = _sessionService.ValidateSession("valid-token");

            //assert
            Assert.IsNotNull(sessionDTO);
            Assert.AreEqual("valid-token", sessionDTO.Token);
        }

        [TestMethod]
        public void ValidateSession_WhenTokenIsInvalid_ThenSessionDataIsCleared()
        {
            //arrange
            SessionDTO session = new SessionDTO
            {
                Token = "invalid-token",
                LoggedUser = new UserDetailDTO { Name = "John" },
                LoggedUserRoleName = "User"
            };

            typeof(SessionService)
                .GetField("_currentSession", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(_sessionService, session);

            JsonElement fakePayload = default;

            _jwtServiceMock.Setup(s => s.ValidateToken(It.IsAny<string>(), It.IsAny<string>(), out fakePayload)).Returns(false);

            //act
            SessionDTO? sessionDTO = _sessionService.ValidateSession("invalid-token");

            //assert
            Assert.IsNull(sessionDTO);
        }

        [TestMethod]
        public void SignOut_WhenCalled_ThenSessionDataIsCleared()
        {
            //arrange
            Role role = new Role(1, "User");
            User user = new User(1, "John", "Doe", "john@test.com", "password123", role);

            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(new List<User> { user });

            _secureDataServiceMock.Setup(s => s.CompareHashes(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            _sessionRepositoryMock.Setup(r => r.AddSession(It.IsAny<Session>()));

            _jwtServiceMock.Setup(t => t.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("valid-token");

            //validateToken must return true before logout
            JsonElement payload = default;

            _jwtServiceMock.Setup(t => t.ValidateToken("valid-token", It.IsAny<string>(), out payload)).Returns(true);

            //create session
            _sessionService.Authenticate("john@test.com", "password");

            //act
            _sessionService.SignOut();

            //simulate validation after logout
            _jwtServiceMock.Setup(t => t.ValidateToken("valid-token", It.IsAny<string>(), out payload)).Returns(false);

            SessionDTO? sessionDTO = _sessionService.ValidateSession("valid-token");

            //assert
            Assert.IsNull(sessionDTO);
        }
    }
}
