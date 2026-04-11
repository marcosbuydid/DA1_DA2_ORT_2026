
using MediaCatalog.Api.Controllers;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using MediaCatalog.Services.Models.GenericWrapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MediaCatalog.Api.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private UserController _userController;
        private List<UserDetailDTO> users;
        UserCreateDTO user;
        UserDetailDTO expectedUser;
        UserUpdateDTO userToUpdate;
        UserDetailDTO expectedUpdatedUser;
        ChangePasswordDTO changePasswordDTO;

        [TestInitialize]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
            _userController = new UserController(_userServiceMock.Object);

            users = new List<UserDetailDTO>()
            {
                new UserDetailDTO() { Name = "Tim", Email = "tim@mail.com" },
                new UserDetailDTO() { Name = "Aubrey", Email = "aubrey@mail.com" }
            };

            user = new UserCreateDTO() { Name = "Aaron", Email = "aaron@mail.com" };
            expectedUser = new UserDetailDTO() { Name = "Aaron", Email = "aaron@mail.com" };

            userToUpdate = new UserUpdateDTO() { Name = "Nick", RoleId = 2 };

            expectedUpdatedUser = new UserDetailDTO()
            {
                Name = "Aaron Updated",
                Email = "aaron@mail.com",
                RoleId = 1
            };

            changePasswordDTO = new ChangePasswordDTO("oldPassword!",
                "newPassw0rd!", "newPassw0rd!");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _userServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Get_WhenCalled_ThenUsersAreReturned()
        {
            //arrange
            _userServiceMock.Setup(us => us.GetUsers()).Returns(users);

            //act
            IActionResult? result = _userController.Get();

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult.");
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<List<UserDetailDTO>>;
            Assert.IsNotNull(response);

            var returnedUsers = response.Result;
            Assert.IsNotNull(returnedUsers);
            Assert.AreEqual(2, returnedUsers.Count);

            Assert.AreEqual("Tim", returnedUsers[0].Name);
            Assert.AreEqual("Aubrey", returnedUsers[1].Name);

            _userServiceMock.Verify(ms => ms.GetUsers(), Times.Once);
        }

        [TestMethod]
        public void GetByEmail_WhenCalled_ThenUserIsReturned()
        {
            //arrange
            _userServiceMock.Setup(us => us.GetUser("aaron@mail.com")).Returns(expectedUser);

            //act
            IActionResult? result = _userController.GetByEmail("aaron@mail.com");

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult.");
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<UserDetailDTO>;
            Assert.IsNotNull(response);

            var returnedUser = response.Result;
            Assert.IsNotNull(returnedUser);

            Assert.AreEqual("aaron@mail.com", returnedUser.Email);

            _userServiceMock.Verify(us => us.GetUser("aaron@mail.com"), Times.Once);
        }

        [TestMethod]
        public void Create_WhenCalled_ThenUserIsCreated()
        {
            //arrange
            _userServiceMock.Setup(us => us.AddUser(It.IsAny<UserCreateDTO>()))
                .Returns(expectedUser);

            //act
            IActionResult? result = _userController.Create(user);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<UserDetailDTO>;
            Assert.IsNotNull(response);

            var createdUser = response.Result;
            Assert.IsNotNull(createdUser);

            Assert.AreEqual("aaron@mail.com", createdUser.Email);

            _userServiceMock.Verify(us => us.AddUser(It.IsAny<UserCreateDTO>()), Times.Once);
        }

        [TestMethod]
        public void Delete_WhenCalled_ThenUserIsDeleted()
        {
            //arrange
            int userId = 1;
            _userServiceMock.Setup(ms => ms.DeleteUserById(userId));

            //act
            IActionResult result = _userController.Delete(userId);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<string>;
            Assert.IsNotNull(response);

            var message = response.Result;
            Assert.IsNotNull(message);

            Assert.AreEqual("User deleted successfully.", message);

            //verify service call
            _userServiceMock.Verify(ms => ms.DeleteUserById(userId), Times.Once);
        }

        [TestMethod]
        public void DeleteByEmail_WhenCalled_ThenUserIsDeleted()
        {
            //arrange
            _userServiceMock.Setup(us => us.DeleteUser(user.Email));

            //act
            IActionResult result = _userController.DeleteByEmail(user.Email);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<string>;
            Assert.IsNotNull(response);

            var message = response.Result;
            Assert.IsNotNull(message);

            Assert.AreEqual("User deleted successfully.", message);

            //verify service call
            _userServiceMock.Verify(us => us.DeleteUser(user.Email), Times.Once);
        }

        [TestMethod]
        public void Update_WhenCalled_ThenUserIsUpdated()
        {
            //arrange
            int userId = 1;
            _userServiceMock.Setup(us => us.UpdateUserById(userId, userToUpdate))
                .Returns(expectedUpdatedUser);

            //act
            IActionResult result = _userController.Update(userId, userToUpdate);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<UserDetailDTO>;
            Assert.IsNotNull(response);

            var returnedUser = response.Result;
            Assert.IsNotNull(returnedUser);

            Assert.AreEqual(expectedUpdatedUser.Name, returnedUser.Name);
            Assert.AreEqual(expectedUpdatedUser.RoleId, returnedUser.RoleId);

            _userServiceMock.Verify(ms => ms.UpdateUserById(userId, userToUpdate), Times.Once);
        }

        [TestMethod]
        public void UpdateByEmail_WhenCalled_ThenUserIsUpdated()
        {
            //arrange
            string userEmail = "aaron@mail.com";
            _userServiceMock.Setup(us => us.UpdateUser(userEmail, userToUpdate))
                .Returns(expectedUpdatedUser);

            //act
            IActionResult result = _userController.UpdateByEmail(userEmail, userToUpdate);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<UserDetailDTO>;
            Assert.IsNotNull(response);

            var returnedUser = response.Result;
            Assert.IsNotNull(returnedUser);

            Assert.AreEqual(expectedUpdatedUser.Name, returnedUser.Name);
            Assert.AreEqual(expectedUpdatedUser.Email, returnedUser.Email);
            Assert.AreEqual(expectedUpdatedUser.RoleId, returnedUser.RoleId);

            _userServiceMock.Verify(us => us.UpdateUser(userEmail, userToUpdate), Times.Once);
        }

        [TestMethod]
        public void ChangePassword_WhenCalled_ThenPasswordIsUpdated()
        {
            //arrange
            string email = "user@test.com";
            _userServiceMock.Setup(us => us.ChangePassword(email, changePasswordDTO));

            //act
            IActionResult result = _userController.ChangePassword(email, changePasswordDTO);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<string>;
            Assert.IsNotNull(response);

            var message = response.Result;
            Assert.IsNotNull(message);
            Assert.AreEqual("Password updated successfully.", message);

            _userServiceMock.Verify(us => us.ChangePassword(email, changePasswordDTO), Times.Once);
        }
    }
}
