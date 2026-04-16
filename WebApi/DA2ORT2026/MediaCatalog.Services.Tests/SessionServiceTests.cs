using MediaCatalog.Domain;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Interfaces.Repositories;
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
        private Role _role;
        private User _user;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _sessionRepositoryMock = new Mock<ISessionRepository>(MockBehavior.Strict);
            _secureDataServiceMock = new Mock<ISecureDataService>(MockBehavior.Strict);

            var systemSettings = Options.Create(new SystemSettings
            {
                EncryptionKey = "abcdefghijklmnopioBpLgpjWR2aHeotXSnsK1234567",
                SecretKey = "ab1c2a3b4c1a2b3c4a1b2c3a4b1c2a3b4c1a2b3c4a1b2="
            });

            _jwtServiceMock = new Mock<ITokenService>(MockBehavior.Strict);

            _sessionService = new SessionService(_sessionRepositoryMock.Object,
                _userRepositoryMock.Object, _secureDataServiceMock.Object, systemSettings,
                _jwtServiceMock.Object);

            _role = new Role(1, "Administrator");
            _user = new User(1, "John", "Doe", "john@test.com", "password123", 1);
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
            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(new List<User> { _user });

            //simulate passwords matches
            _secureDataServiceMock.Setup(s => s.CompareHashes(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            _sessionRepositoryMock.Setup(r => r.AddSession(It.IsAny<Session>()));

            _jwtServiceMock.Setup(t => t.GenerateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<int>())).Returns("fake-token");

            //act
            _sessionService.Authenticate("john@test.com", "password");

            //assert
            _sessionRepositoryMock.Verify(r => r.AddSession(It.IsAny<Session>()), Times.Once);
        }

        [TestMethod]
        public void Authenticate_WhenCredentialsAreInvalid_ThenThrowsException()
        {
            //arrange
            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(new List<User> { _user });

            //password does not match
            _secureDataServiceMock.Setup(s => s.CompareHashes(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            //act
            Action action = () => _sessionService.Authenticate("john@mail.com", "wrongPassword");

            //assert
            Assert.ThrowsException<ServiceException>(action);

            _sessionRepositoryMock.Verify(r => r.AddSession(It.IsAny<Session>()), Times.Never);
        }

        [TestMethod]
        public void ValidateSession_WhenTokenIsValid_ThenSessionIsReturned()
        {
            //arrange
            string token = "valid-token";

            //JWT payload
            string tokenPayload = @"{""name"": ""John"",""lastName"": ""Doe"",
                ""email"": ""john@example.com"",""roleName"": ""User"",""roleId"": 1,
                ""iss"": ""api"", ""aud"": ""webapp""}";
            JsonElement payload = JsonDocument.Parse(tokenPayload).RootElement;

            //mock ITokenService.ValidateToken to return true and set out parameter
            _jwtServiceMock.Setup(s => s.ValidateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), out It.Ref<JsonElement>.IsAny)).Returns(true).Callback((string t, string k,
                string expectedAudience, string expectedIssuer, out JsonElement p) =>
                { p = payload; });

            //act
            SessionDTO? sessionDTO = _sessionService.ValidateSession(token);

            //assert
            Assert.IsNotNull(sessionDTO);
            Assert.AreEqual(token, sessionDTO.Token);
            Assert.AreEqual("John", sessionDTO.LoggedUser.Name);
            Assert.AreEqual("Doe", sessionDTO.LoggedUser.LastName);
            Assert.AreEqual("User", sessionDTO.LoggedUserRoleName);
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

            _jwtServiceMock.Setup(s => s.ValidateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                out fakePayload)).Returns(false);

            //act
            SessionDTO? sessionDTO = _sessionService.ValidateSession("invalid-token");

            //assert
            Assert.IsNull(sessionDTO);
        }
    }
}
