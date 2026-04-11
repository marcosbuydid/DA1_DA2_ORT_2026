
using MediaCatalog.Api.Controllers;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using MediaCatalog.Services.Models.GenericWrapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MediaCatalog.Api.Tests
{
    [TestClass]
    public class SessionControllerTests
    {
        private Mock<ISessionService> _sessionServiceMock;
        private SessionController _sessionController;

        [TestInitialize]
        public void Setup()
        {
            _sessionServiceMock = new Mock<ISessionService>(MockBehavior.Strict);
            _sessionController = new SessionController(_sessionServiceMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _sessionServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Login_WhenCalled_ThenAccessTokenIsReturned()
        {
            //arrange
            LoginUserDTO loginUserDTO = new LoginUserDTO("test@mail.com", "password123");
            string expectedToken = "jwt-token";

            _sessionServiceMock.Setup(s => s.Authenticate(loginUserDTO.Email, loginUserDTO.Password))
                .Returns(expectedToken);

            //act
            IActionResult? result = _sessionController.Login(loginUserDTO);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<string>;
            Assert.IsNotNull(response);

            Assert.AreEqual(expectedToken, response.Result);

            _sessionServiceMock.Verify(s => s.Authenticate(loginUserDTO.Email, 
                loginUserDTO.Password),Times.Once);
        }

        [TestMethod]
        public void Login_WhenCredentialsAreInvalid_ThenThrowsException()
        {
            // arrange
            LoginUserDTO loginUserDTO = new LoginUserDTO("test@mail.com", "aPassword!");

            _sessionServiceMock.Setup(s => s.Authenticate(It.IsAny<string>(), 
                It.IsAny<string>())).Throws(new ServiceException("Invalid credentials"));

            // act
            Action action = () => _sessionController.Login(loginUserDTO);

            // assert
            Assert.ThrowsException<ServiceException>(action);
        }
    }
}
