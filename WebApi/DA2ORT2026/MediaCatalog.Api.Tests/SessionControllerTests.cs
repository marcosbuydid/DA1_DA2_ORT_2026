
using MediaCatalog.Api.Controllers;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using MediaCatalog.Services.Models.GenericWrapper;
using Microsoft.AspNetCore.Http;
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
        public void GetCurrentSession_WhenCalled_ThenSessionIsReturned()
        {
            //arrange
            SessionDTO expectedSession = new SessionDTO
            {
                Token = "fake-token",
                LoggedUser = new UserDetailDTO
                {
                    Name = "John",
                    LastName = "Doe",
                    Email = "john@test.com",
                    RoleId = 1
                },
                LoggedUserRoleName = "Administrator"
            };

            _sessionServiceMock
                .Setup(s => s.ValidateSession("fake-token"))
                .Returns(expectedSession);

            //fake HTTP context with header
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = "Bearer fake-token";

            _sessionController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            //act
            var result = _sessionController.GetCurrentSession();

            //assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var response = okResult.Value as ApiResponse<SessionDTO>;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Result);

            Assert.AreEqual("John", response.Result.LoggedUser.Name);
            Assert.AreEqual("Doe", response.Result.LoggedUser.LastName);
        }
    }
}
