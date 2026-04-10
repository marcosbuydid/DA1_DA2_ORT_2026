
using MediaCatalog.Api.Controllers;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MediaCatalog.Api.Tests
{
    [TestClass]
    public class RoleControllerTests
    {
        private Mock<IRoleService> _roleServiceMock;
        private RoleController _roleController;

        [TestInitialize]
        public void Setup()
        {
            _roleServiceMock = new Mock<IRoleService>(MockBehavior.Strict);
            _roleController = new RoleController(_roleServiceMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _roleServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Get_WhenCalledThenRolesAreReturned()
        {
            //arrange
            List<RoleDetailDTO> roles = new List<RoleDetailDTO>()
            { new RoleDetailDTO() { Name = "Administrator" },
              new RoleDetailDTO() { Name = "User" }};

            _roleServiceMock.Setup(rs => rs.GetRoles()).Returns(roles);

            //act
            IActionResult? result = _roleController.Get();

            //assert
            Assert.IsNotNull(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult.");
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedRoles = okResult.Value;
            Assert.IsNotNull(returnedRoles);

            _roleServiceMock.Verify(rs => rs.GetRoles(), Times.Once);
        }

        [TestMethod]
        public void GetByName_WhenCalledThenRoleIsReturned()
        {
            //arrange
            RoleDetailDTO expectedRole = new RoleDetailDTO() { Name = "Administrator" };

            _roleServiceMock.Setup(rs => rs.GetRole("Administrator")).Returns(expectedRole);

            //act
            IActionResult? result = _roleController.GetByName("Administrator");

            // assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult.");
            Assert.AreEqual(200, okResult.StatusCode);

            _roleServiceMock.Verify(rs => rs.GetRole("Administrator"), Times.Once);
        }

        [TestMethod]
        public void Create_WhenCalledThenRoleIsCreated()
        {
            // arrange
            RoleCreateDTO role = new RoleCreateDTO("User");
            RoleDetailDTO expectedRole = new RoleDetailDTO(1, "User");

            _roleServiceMock
                .Setup(rs => rs.AddRole(It.IsAny<RoleCreateDTO>()))
                .Returns(expectedRole);

            // act
            IActionResult? result = _roleController.Create(role);

            // assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            _roleServiceMock.Verify(rs => rs.AddRole(It.IsAny<RoleCreateDTO>()), Times.Once);
        }

        [TestMethod]
        public void Delete_WhenCalled_ThenRoleIsDeleted()
        {
            //arrange
            int roleId = 1;
            _roleServiceMock.Setup(rs => rs.DeleteRoleById(roleId));

            //act
            IActionResult result = _roleController.Delete(roleId);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            //verify response message
            var value = okResult.Value;
            Assert.IsNotNull(value);

            var messageProperty = value.GetType().GetProperty("Message");
            Assert.IsNotNull(messageProperty);

            var message = messageProperty.GetValue(value)?.ToString();
            Assert.AreEqual("Role deleted successfully.", message);

            // verify service call
            _roleServiceMock.Verify(rs => rs.DeleteRoleById(roleId), Times.Once);
        }

        [TestMethod]
        public void DeleteByName_WhenCalled_ThenRoleIsDeleted()
        {
            //arrange
            RoleCreateDTO role = new RoleCreateDTO("User");
            _roleServiceMock.Setup(rs => rs.DeleteRole(role.Name));

            //act
            IActionResult result = _roleController.DeleteByName(role.Name);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            //verify response message
            var value = okResult.Value;
            Assert.IsNotNull(value);

            var messageProperty = value.GetType().GetProperty("Message");
            Assert.IsNotNull(messageProperty);

            var message = messageProperty.GetValue(value)?.ToString();
            Assert.AreEqual("Role deleted successfully.", message);

            // verify service call
            _roleServiceMock.Verify(rs => rs.DeleteRole(role.Name), Times.Once);
        }
    }
}