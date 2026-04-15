
using MediaCatalog.Domain;
using MediaCatalog.Services.Interfaces.Repositories;
using Moq;

namespace MediaCatalog.Services.Tests
{
    [TestClass]
    public class JWTServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IRoleRepository> _roleRepositoryMock;
        private JWTService _jwtService;

        private const string SecretKey = "secret_key_12345";

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _roleRepositoryMock = new Mock<IRoleRepository>(MockBehavior.Strict);

            User user = new User(1, "John", "Doe", "john@test.com", "password.234", 1);
            Role role = new Role(1, "Administrator");

            List<User> users = new List<User>() { user };
            List<Role> roles = new List<Role>() { role };

            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(users);
            _roleRepositoryMock.Setup(r => r.GetRoles()).Returns(roles);

            _jwtService = new JWTService(_userRepositoryMock.Object, _roleRepositoryMock.Object);
        }

        [TestMethod]
        public void GenerateToken_WhenCalled_ThenATokenIsReturned()
        {
            //arrange
            //act
            var token = _jwtService.GenerateToken("John", "john@test.com", SecretKey, 10);

            //assert
            Assert.IsNotNull(token);
        }

        [TestMethod]
        public void GenerateToken_WhenCalled_ThenTokenWithThreePartsIsReturned()
        {
            //arrange
            //act
            var token = _jwtService.GenerateToken("John", "john@test.com", SecretKey, 10);

            //assert
            string[] parts = token.Split('.');
            Assert.AreEqual(3, parts.Length);
        }

        [TestMethod]
        public void ValidateToken_WhenCalledWithNullToken_ThenFalseIsReturned()
        {
            //act
            bool result = _jwtService.ValidateToken(null, SecretKey, "MediaCatalogAPI",
                "MediaCatalogWebApp", out _);

            //assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateToken_WhenCalled_ThenTrueIsReturned()
        {
            //arrange
            string token = _jwtService.GenerateToken("John", "john@test.com", SecretKey, 10);

            //act
            bool result = _jwtService.ValidateToken(token, SecretKey, "MediaCatalogAPI",
                "MediaCatalogWebApp", out var payload);

            //assert
            Assert.IsTrue(result);
            Assert.IsTrue(payload.TryGetProperty("email", out var email));
            Assert.AreEqual("john@test.com", email.GetString());
        }

        [TestMethod]
        public void ValidateToken_WhenCalledWithInvalidSignature_ThenFalseIsReturned()
        {
            //arrange
            string token = _jwtService.GenerateToken("John", "john@test.com", SecretKey, 10);

            //tamper token
            string tamperedToken = token.Replace("a", "b");

            //act
            bool result = _jwtService.ValidateToken(tamperedToken, SecretKey, "MediaCatalogAPI",
                "MediaCatalogWebApp", out _);

            //assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateToken_WhenCalledWithWrongIssuer_ThenFalseIsReturned()
        {
            //arrange
            string token = _jwtService.GenerateToken("John", "john@test.com", SecretKey, 10);

            //act
            bool result = _jwtService.ValidateToken(token, SecretKey, "WrongIssuer", "MediaCatalogWebApp", out _);

            //assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateToken_WhenCalledWithWrongAudience_ThenFalseIsReturned()
        {
            //arrange
            string token = _jwtService.GenerateToken("John", "john@test.com", SecretKey, 10);

            //act
            bool result = _jwtService.ValidateToken(token, SecretKey, "MediaCatalogAPI", "WrongAudience", out _);

            //assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateToken_WhenCalledWithExpiredToken_ThenFalseIsReturned()
        {
            //arrange
            //token already expired
            string token = _jwtService.GenerateToken("John", "john@test.com", SecretKey, -1);

            //act
            bool result = _jwtService.ValidateToken(token, SecretKey, "MediaCatalogAPI", "MediaCatalogWebApp", out _);

            //assert
            Assert.IsFalse(result);
        }
    }
}
