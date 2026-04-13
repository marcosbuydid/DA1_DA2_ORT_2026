
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Models;
using Moq;
using System.Linq.Expressions;

namespace MediaCatalog.Services.Tests
{
    [TestClass]
    public class RoleServiceTests
    {
        private Mock<IRoleRepository> _roleRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private RoleService _roleService;
        private Role _role;
        private RoleCreateDTO _createRoleDTO;
        private RoleDetailDTO _detailRoleDTO;
        private List<Role> _roles;

        [TestInitialize]
        public void Setup()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>(MockBehavior.Strict);
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
            _roleService = new RoleService(_roleRepositoryMock.Object, _userRepositoryMock.Object);
            _role = new Role { Id = 1, Name = "Administrator" };
            _createRoleDTO = new RoleCreateDTO("Administrator");
            _detailRoleDTO = new RoleDetailDTO { Name = "Owner" };
            _roles = new List<Role>()
            {
                new Role(1,"Administrator"),
                new Role(2, "User")
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _roleRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetRole_WhenCalled_ThenRoleIsReturned()
        {
            //arrange
            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns(_role);

            //act
            RoleDetailDTO roleDTO = _roleService.GetRole(_role.Name);

            //assert
            Assert.AreEqual(roleDTO.Name, _role.Name);

            _roleRepositoryMock.Verify(r => r.GetRole(It.IsAny<Func<Role, bool>>()), Times.Once());
        }

        [TestMethod]
        public void GetRole_WhenCalledWithUnregisteredRole_ThenThrowsException()
        {
            //arrange
            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns((Role)null);

            //act
            Action action = () => _roleService.GetRole(_createRoleDTO.Name);

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(() => _roleService.GetRole(_detailRoleDTO.Name));

            _roleRepositoryMock.Verify(r => r.Exists(It.IsAny<Expression<Func<Role, bool>>>()), Times.Never);
        }

        [TestMethod]
        public void AddRole_WhenCalledWithANotAllowedName_ThenThrowsException()
        {
            //arrange
            _roleRepositoryMock.Setup(r => r.GetRoles()).Returns(_roles);

            //act
            Action action = () => _roleService.AddRole(_createRoleDTO);

            //assert
            Assert.ThrowsException<ConflictException>(action);

            _roleRepositoryMock.Verify(r => r.Exists(It.IsAny<Expression<Func<Role, bool>>>()), Times.Never);
        }

        [TestMethod]
        public void AddUser_WhenCalled_ThenRoleIsAdded()
        {
            //arrange
            _roleRepositoryMock.Setup(r => r.GetRoles()).Returns(new List<Role>());
            _roleRepositoryMock.Setup(r => r.AddRole(It.IsAny<Role>()));

            //act
            RoleDetailDTO roleAdded = _roleService.AddRole(_createRoleDTO);

            //assert
            Assert.AreEqual(_createRoleDTO.Name, roleAdded.Name);

            _roleRepositoryMock.Verify(r => r.Exists(It.IsAny<Expression<Func<Role, bool>>>()), Times.Never);
        }

        public void AddRole_WhenCalledWithANameAlreadyInUse_ThenThrowsException()
        {
            //arrange
            //simulate role with the same name already exists in repository
            List<Role> existingRoles = new List<Role>();
            Role role = new Role(1, "Administrator");
            existingRoles.Add(role);

            _roleRepositoryMock.Setup(r => r.GetRoles()).Returns(existingRoles);

            //act
            Action action = () => _roleService.AddRole(_createRoleDTO);

            //assert
            Assert.ThrowsException<ConflictException>(action);
            _roleRepositoryMock.Verify(r => r.AddRole(It.IsAny<Role>()), Times.Never);
        }

        [TestMethod]
        public void GetRoles_WhenCalled_ThenRolesAreReturned()
        {
            //arrange
            _roleRepositoryMock.Setup(r => r.GetRoles()).Returns(_roles);

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
            Assert.ThrowsException<ResourceNotFoundException>(action);

            _roleRepositoryMock.Verify(r => r.DeleteRole(It.IsAny<Role>()), Times.Never);
        }

        [TestMethod]
        public void DeleteRoleById_WhenCalledWithInvalidId_ThenThrowsException()
        {
            //arrange
            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns((Role)null);

            //act
            Action action = () => _roleService.DeleteRoleById(50);

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(action);

            _roleRepositoryMock.Verify(r => r.DeleteRole(It.IsAny<Role>()), Times.Never);
        }

        [TestMethod]
        public void DeleteRole_WhenCalled_ThenRoleIsDeleted()
        {
            //arrange
            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns(_role);
            _roleRepositoryMock.Setup(r => r.DeleteRole(It.IsAny<Role>()));

            //simulate role is not been used by any user
            _userRepositoryMock.Setup(u => u.Exists(It.IsAny<Expression<Func<User, bool>>>())).Returns(false);

            //act
            _roleService.DeleteRole(_role.Name);

            //assert
            _roleRepositoryMock.Verify(r => r.DeleteRole(It.Is<Role>(ro => ro.Name == "Administrator")), Times.Once);
        }

        [TestMethod]
        public void DeleteRoleById_WhenCalledWithValidId_ThenRoleIsDeleted()
        {
            //arrange
            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns(_role);
            _roleRepositoryMock.Setup(r => r.DeleteRole(It.IsAny<Role>()));

            //simulate role is not been used by any user
            _userRepositoryMock.Setup(u => u.Exists(It.IsAny<Expression<Func<User, bool>>>())).Returns(false);

            //act
            _roleService.DeleteRoleById(_role.Id);

            //assert
            _roleRepositoryMock.Verify(r => r.DeleteRole(It.Is<Role>(ro => ro.Id == _role.Id)), Times.Once);
        }

        [TestMethod]
        public void DeleteRole_WhenCalledWithRoleInUse_ThenThrowsException()
        {
            //arrange
            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns(_role);

            //simulate role is being used by user
            _userRepositoryMock.Setup(u => u.Exists(It.IsAny<Expression<Func<User, bool>>>())).Returns(true);

            //act and assert
            Assert.ThrowsException<ServiceException>(() => _roleService.DeleteRole(_role.Name));

            //verify delete was never called
            _roleRepositoryMock.Verify(r => r.DeleteRole(It.IsAny<Role>()), Times.Never);
        }

        [TestMethod]
        public void DeleteRoleById_WhenCalledWithRoleInUse_ThenThrowsException()
        {
            //arrange
            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns(_role);

            //simulate role is being used by user
            _userRepositoryMock.Setup(u => u.Exists(It.IsAny<Expression<Func<User, bool>>>())).Returns(true);

            //act and assert
            Assert.ThrowsException<ServiceException>(() => _roleService.DeleteRoleById(_role.Id));

            //verify delete was never called
            _roleRepositoryMock.Verify(r => r.DeleteRole(It.IsAny<Role>()), Times.Never);
        }
    }
}
