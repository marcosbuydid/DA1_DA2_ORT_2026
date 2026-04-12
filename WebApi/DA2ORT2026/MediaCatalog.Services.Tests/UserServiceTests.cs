
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using MediaCatalog.Domain.Exceptions;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using Moq;
using System.Linq.Expressions;


namespace MediaCatalog.Services.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IRoleRepository> _roleRepositoryMock;
        private Mock<ISecureDataService> _secureDataServiceMock;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _roleRepositoryMock = new Mock<IRoleRepository>(MockBehavior.Strict);
            _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);

            _secureDataServiceMock = new Mock<ISecureDataService>(MockBehavior.Strict);

            _userService = new UserService(_userRepositoryMock.Object, _roleRepositoryMock.Object,
                _secureDataServiceMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _userRepositoryMock.VerifyAll();
            _roleRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void AddUser_WhenCalledWithInvalidUser_ThenThrowsException()
        {
            //arrange
            Role role = new Role() { Id = 1 };

            UserCreateDTO newUserDTO = new UserCreateDTO("John", "Doe", "test", "password123", (int)role.Id);

            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns(role);

            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(new List<User>());

            //setup hash method
            _secureDataServiceMock.Setup(s => s.Hash(It.IsAny<string>())).Returns((string s) => "hashed-" + s);

            //act
            Action action = () => _userService.AddUser(newUserDTO);

            //assert
            Assert.ThrowsException<DomainException>(() => _userService.AddUser(newUserDTO));

            //verify that no existence check was performed (as per your expectation)
            _userRepositoryMock.Verify(r => r.Exists(It.IsAny<Expression<Func<User, bool>>>()), Times.Never);
        }

        [TestMethod]
        public void AddUser_WhenCalled_ThenUserIsAdded()
        {
            //arrange
            Role role = new Role { Id = 1 };

            UserCreateDTO newUserDTO = new UserCreateDTO("John", "Doe", "john.doe@test.com", "password123", (int)role.Id);

            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns(role);

            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(new List<User>());

            _userRepositoryMock.Setup(r => r.AddUser(It.IsAny<User>()));

            //setup hash method
            _secureDataServiceMock.Setup(s => s.Hash(It.IsAny<string>())).Returns((string s) => "hashed-" + s);

            //act
            UserDetailDTO userAdded = _userService.AddUser(newUserDTO);

            //assert
            _userRepositoryMock.Verify(
                r => r.AddUser(It.Is<User>(u =>
                    u.Name == newUserDTO.Name &&
                    u.LastName == newUserDTO.LastName &&
                    u.Email == newUserDTO.Email &&
                    u.Role != null &&
                    u.Password != "password123" //to ensure it was hashed
                )),
                Times.Once
            );

            Assert.AreEqual(newUserDTO.Name, userAdded.Name);
            Assert.AreEqual(newUserDTO.LastName, userAdded.LastName);
            Assert.AreEqual(newUserDTO.Email, userAdded.Email);
            Assert.AreEqual(newUserDTO.RoleId, userAdded.RoleId);
        }

        [TestMethod]
        public void AddUser_WhenCalledWithAnEmailAlreadyInUse_ThenThrowsException()
        {
            //arrange
            Role role = new Role { Id = 1 };

            UserCreateDTO newUserDTO = new UserCreateDTO("Nick", "Smith", "jane.smith@test.com", "password123", (int)role.Id);

            //simulate user with the same email already exists in repository
            List<User> existingUsers = new List<User>();
            User newUser = new User(2, "Jane", "Smith", "jane.smith@test.com", "hashed123", role);
            existingUsers.Add(newUser);

            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(existingUsers);

            //act
            Action action = () => _userService.AddUser(newUserDTO);

            //assert
            Assert.ThrowsException<ConflictException>(action);
            _userRepositoryMock.Verify(r => r.AddUser(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void GetUser_WhenCalled_ThenUserIsReturned()
        {
            //arrange
            Role role = new Role { Id = 1 };

            User newUser = new User(10, "Jim", "Adks", "jim.adks@example.com", "password123", role);

            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns(newUser);

            //act
            UserDetailDTO userDTO = _userService.GetUser(newUser.Email);

            //assert
            Assert.AreEqual(userDTO.Name, newUser.Name);
            Assert.AreEqual(userDTO.LastName, newUser.LastName);
            Assert.AreEqual(userDTO.Email, newUser.Email);
            Assert.AreEqual(userDTO.RoleId, newUser.Role.Id);

            _userRepositoryMock.Verify(r => r.GetUser(It.IsAny<Func<User, bool>>()), Times.Once());
        }

        [TestMethod]
        public void GetUser_WhenCalledWithNonRegisteredUser_ThenThrowsException()
        {
            //arrange
            Role role = new Role { Id = 1 };

            UserCreateDTO newUserDTO = new UserCreateDTO("Jim", "Adks", "jim.adks@example.com", "password123", (int)role.Id);

            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns((User)null);

            //act
            Action action = () => _userService.GetUser(newUserDTO.Email);

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(() => _userService.GetUser(newUserDTO.Email));

            _userRepositoryMock.Verify(r => r.Exists(It.IsAny<Expression<Func<User, bool>>>()), Times.Never);
        }

        [TestMethod]
        public void GetUsers_WhenCalled_ThenUsersAreReturned()
        {
            //arrange
            Role role = new Role { Id = 1 };

            List<User> users = new List<User>()
            {
                new User(1,"John","Doe","john@test.com", "password1234",role),
                new User(1, "Jane", "Smith", "jane@test.com","password4321",role)
            };

            _userRepositoryMock.Setup(r => r.GetUsers()).Returns(users);

            //act
            List<UserDetailDTO> result = _userService.GetUsers();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("John", result[0].Name);
            Assert.AreEqual("Jane", result[1].Name);

            _userRepositoryMock.Verify(r => r.GetUsers(), Times.Once);
        }

        [TestMethod]
        public void DeleteUser_WhenCalledWithUnregisteredUser_ThenThrowsException()
        {
            //arrange
            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns((User)null);

            //act
            Action action = () => _userService.DeleteUser("email@test.com");

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(action);

            _userRepositoryMock.Verify(r => r.DeleteUser(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void DeleteUser_WhenCalledWithInvalidId_ThenThrowsException()
        {
            //arrange
            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns((User)null);

            //act
            Action action = () => _userService.DeleteUserById(25);

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(action);

            _userRepositoryMock.Verify(r => r.DeleteUser(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void DeleteUser_WhenCalled_ThenUserIsDeleted()
        {
            //arrange
            Role role = new Role { Id = 1 };

            User user = new User(1, "John", "Miller", "john@email.com", "hashed124", role);

            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns(user);

            _userRepositoryMock.Setup(r => r.DeleteUser(It.IsAny<User>()));

            //act
            _userService.DeleteUser(user.Email);

            //assert
            _userRepositoryMock.Verify(r => r.DeleteUser(It.Is<User>(u => u.Email == "john@email.com")), Times.Once);
        }

        [TestMethod]
        public void DeleteUser_WhenCalledWithValidId_ThenUserIsDeleted()
        {
            //arrange
            Role role = new Role { Id = 1 };

            User user = new User(1, "John", "Miller", "john@email.com", "hashed124", role);

            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns(user);

            _userRepositoryMock.Setup(r => r.DeleteUser(It.IsAny<User>()));

            //act
            _userService.DeleteUserById(user.Id);

            //assert
            _userRepositoryMock.Verify(r => r.DeleteUser(It.Is<User>(u => u.Id == user.Id)), Times.Once);
        }

        [TestMethod]
        public void UpdateUser_WhenCalledWithUnregisteredUser_ThenThrowsException()
        {
            //arrange
            Role role = new Role { Id = 1 };

            UserUpdateDTO userDTO = new UserUpdateDTO("aName", "aLastName", "email@test.com", (int)role.Id);

            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns((User)null);

            //act
            Action action = () => _userService.UpdateUser("undefined@mail.com", userDTO);

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(action);

            _userRepositoryMock.Verify(r => r.UpdateUser(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void UpdateUser_WhenCalledWithInvalidId_ThenThrowsException()
        {
            //arrange
            Role role = new Role { Id = 1 };

            UserUpdateDTO userDTO = new UserUpdateDTO("aName", "aLastName", "email@test.com", (int)role.Id);

            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns((User)null);

            //act
            Action action = () => _userService.UpdateUserById(25, userDTO);

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(action);

            _userRepositoryMock.Verify(r => r.UpdateUser(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void UpdateUser_WhenCalled_ThenUserIsUpdated()
        {
            //arrange
            Role role = new Role { Id = 1 };

            User existingUser = new User(1, "OldName", "OldLastName", "oldemail@mail.com", "hashed1234", role);

            UserUpdateDTO userToUpdate = new UserUpdateDTO("NewName", "NewLastName", "newemail@email.com", (int)role.Id);

            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns(role);

            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUser(It.IsAny<User>()));

            //act
            UserDetailDTO updatedUser = _userService.UpdateUser(existingUser.Email, userToUpdate);

            //assert
            _userRepositoryMock.Verify(
                r => r.UpdateUser(It.Is<User>(u =>
                    u.Name == userToUpdate.Name &&
                    u.LastName == userToUpdate.LastName &&
                    u.Role.Id == userToUpdate.RoleId
                )), Times.Once);

            Assert.AreEqual(userToUpdate.Name, updatedUser.Name);
            Assert.AreEqual(userToUpdate.LastName, updatedUser.LastName);
            Assert.AreEqual(userToUpdate.RoleId, updatedUser.RoleId);
        }

        [TestMethod]
        public void UpdateUser_WhenCalledWithValidId_ThenUserIsUpdated()
        {
            //arrange
            Role role = new Role { Id = 1 };

            User existingUser = new User(1, "OldName", "OldLastName", "oldemail@mail.com", "hashed1234", role);

            UserUpdateDTO userToUpdate = new UserUpdateDTO("NewName", "NewLastName", "newemail@email.com", (int)role.Id);

            _roleRepositoryMock.Setup(r => r.GetRole(It.IsAny<Func<Role, bool>>())).Returns(role);

            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns(existingUser);

            _userRepositoryMock.Setup(r => r.UpdateUser(It.IsAny<User>()));

            //act
            UserDetailDTO updatedUser = _userService.UpdateUserById(1, userToUpdate);

            //assert
            _userRepositoryMock.Verify(
                r => r.UpdateUser(It.Is<User>(u =>
                    u.Name == userToUpdate.Name &&
                    u.LastName == userToUpdate.LastName &&
                    u.Role.Id == userToUpdate.RoleId
                )), Times.Once);

            Assert.AreEqual(userToUpdate.Name, updatedUser.Name);
            Assert.AreEqual(userToUpdate.LastName, updatedUser.LastName);
            Assert.AreEqual(userToUpdate.RoleId, updatedUser.RoleId);
        }

        [TestMethod]
        public void ChangePassword_WhenCalledAndOldPasswordNotMatch_ThenThrowsException()
        {
            //arrange
            Role role = new Role { Id = 1 };

            string email = "john@test.com";
            string oldPassword = "oldPassword";
            string newPassword = "newPassword123";

            ChangePasswordDTO changePasswordDTO =
                new ChangePasswordDTO(oldPassword, newPassword, newPassword);

            //simulate validation failure, user = null
            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns((User)null);

            //act
            Action action = () => _userService.ChangePassword(email, changePasswordDTO);

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(action);

            _userRepositoryMock.Verify(r => r.UpdateUser(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public void ChangePassword_WhenCalled_ThenPasswordIsUpdated()
        {
            //arrange
            Role role = new Role { Id = 1 };

            string email = "john@test.com";
            string oldPassword = "oldPassword";
            string newPassword = "newPassword123";

            //setup hash method
            _secureDataServiceMock.Setup(s => s.Hash(It.IsAny<string>())).Returns((string s) => "hashed-" + s);

            //setup compareHashes
            _secureDataServiceMock.Setup(s => s.CompareHashes(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string hashed, string plain) => hashed == "hashed-" + plain);

            string hashedOldPassword = _secureDataServiceMock.Object.Hash(oldPassword);

            User user = new User(1, "John", "Doe", email, hashedOldPassword, role);

            ChangePasswordDTO changePasswordDTO =
                new ChangePasswordDTO(oldPassword, newPassword, newPassword);

            //mock repository to return user
            _userRepositoryMock.Setup(r => r.GetUser(It.IsAny<Func<User, bool>>())).Returns(user);

            User updatedUser = null;

            _userRepositoryMock.Setup(r => r.UpdateUser(It.IsAny<User>())).Callback<User>(u => updatedUser = u);

            //act
            _userService.ChangePassword(email, changePasswordDTO);

            //assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(email, updatedUser.Email);
            Assert.AreNotEqual(hashedOldPassword, updatedUser.Password);
        }
    }
}
