
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using MediaCatalog.Domain.Exceptions;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Models;
using Moq;

namespace MediaCatalog.Services.Tests
{
    [TestClass]
    public class RoleServiceTests
    {
        private Mock<IRoleRepository> _roleRepositoryMock;
        private RoleService _roleService;

        [TestInitialize]
        public void Setup()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>(MockBehavior.Strict);
            _roleService = new RoleService(_roleRepositoryMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _roleRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetRole_WhenCalledThenRoleIsReturned()
        {
            //arrange
            Role role = new Role { Id = 1, Name = "User" };

            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns(role);

            //act
            RoleDetailDTO roleDTO = _roleService.GetRole(role.Name);

            //assert
            Assert.AreEqual(roleDTO.Name, role.Name);

            _roleRepositoryMock.Verify(r => r.GetRole(It.IsAny<Func<Role, bool>>()), Times.Once());
        }

        [TestMethod]
        public void GetRole_WhenCalledWithUnregisteredRoleThenThrowsException()
        {
            //arrange
            Role role = new Role { Id = 1, Name = "User" };

            RoleDetailDTO roleDTO = new RoleDetailDTO(10, "User");

            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns((Role)null);

            //act
            Action action = () => _roleService.GetRole(roleDTO.Name);

            //assert
            Assert.ThrowsException<ServiceException>(() => _roleService.GetRole(roleDTO.Name));
            _roleRepositoryMock.Verify(r => r.Exists(It.IsAny<Func<Role, bool>>()), Times.Never);
        }

        [TestMethod]
        public void AddRole_WhenCalledWithANotAllowedNameThenThrowsException()
        {
            //arrange
            RoleCreateDTO roleDTO = new RoleCreateDTO {Name = "Owner" };

            _roleRepositoryMock.Setup(r => r.GetRoles()).Returns(new List<Role>());

            //act
            Action action = () => _roleService.AddRole(roleDTO);

            //assert
            Assert.ThrowsException<DomainException>(action);
            //verify that no existence check was performed (as per your expectation)
            _roleRepositoryMock.Verify(r => r.Exists(It.IsAny<Func<Role, bool>>()), Times.Never);
        }

        [TestMethod]
        public void AddUser_WhenCalled_ThenRoleIsAdded()
        {
            //arrange
            RoleCreateDTO roleDTO = new RoleCreateDTO { Name = "Administrator" };

            _roleRepositoryMock.Setup(r => r.GetRoles()).Returns(new List<Role>());

            _roleRepositoryMock.Setup(r => r.AddRole(It.IsAny<Role>()));

            //act
            RoleDetailDTO roleAdded = _roleService.AddRole(roleDTO);

            //assert
            _roleRepositoryMock.Verify(r => r.Exists(It.IsAny<Func<Role, bool>>()), Times.Never);

            Assert.AreEqual(roleDTO.Name, roleAdded.Name);
        }

        public void AddRole_WhenCalledWithANameAlreadyInUse_ThenThrowsException()
        {
            //arrange
            RoleCreateDTO roleDTO = new RoleCreateDTO { Name = "Administrator" };

            //simulate role with the same name already exists in repository
            List<Role> existingRoles = new List<Role>();
            Role role = new Role(1, "Administrator");
            existingRoles.Add(role);

            _roleRepositoryMock.Setup(r => r.GetRoles()).Returns(existingRoles);

            //act
            Action action = () => _roleService.AddRole(roleDTO);

            //assert
            Assert.ThrowsException<ServiceException>(action);
            _roleRepositoryMock.Verify(r => r.AddRole(It.IsAny<Role>()), Times.Never);
        }

        [TestMethod]
        public void GetRoles_WhenCalled_ThenRolesAreReturned()
        {
            //arrange
            List<Role> roles = new List<Role>()
            {
                new Role(1,"Administrator"),
                new Role(2, "User")
            };

            _roleRepositoryMock.Setup(r => r.GetRoles()).Returns(roles);

            //act
            List<RoleDetailDTO> result = _roleService.GetRoles();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Administrator", result[0].Name);
            Assert.AreEqual("User", result[1].Name);

            _roleRepositoryMock.Verify(r => r.GetRoles(), Times.Once);
        }

        [TestMethod]
        public void DeleteRole_WhenCalledWithUnregisteredName_ThenThrowsException()
        {
            //arrange
            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns((Role)null);

            //act
            Action action = () => _roleService.DeleteRole("aRoleName");

            //assert
            Assert.ThrowsException<ServiceException>(action);

            _roleRepositoryMock.Verify(r => r.DeleteRole(It.IsAny<Role>()), Times.Never);
        }

        [TestMethod]
        public void DeleteRole_WhenCalled_ThenRoleIsDeleted()
        {
            //arrange
            Role role = new Role { Id = 1, Name = "User" };

            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns(role);

            _roleRepositoryMock.Setup(r => r.DeleteRole(It.IsAny<Role>()));

            //act
            _roleService.DeleteRole(role.Name);

            //assert
            _roleRepositoryMock.Verify(r => r.DeleteRole(It.Is<Role>(ro => ro.Name == "User")), Times.Once);
        }
    }
}
