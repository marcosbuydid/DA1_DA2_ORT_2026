
using MediaCatalog.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaCatalog.Domain.Tests
{
    [TestClass]
    public class UserTests
    {
        private User _user;
        private Role _role;

        [TestInitialize]
        public void Initialize()
        {
            _role = new Role(1, "Administrator");
            _user = new User(1, "FirstName", "LastName", "email@example.com", "password1234", 1);
        }


        [TestMethod]
        public void CreateNewUser_WhenNameIsEmptyThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _user.Name = "");
            Assert.AreEqual("Name cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewUser_WhenNameIsNullThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _user.Name = null);
            Assert.AreEqual("Name cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewUser_WhenNameIsNotNullOrEmptyThenNameIsAssigned()
        {
            Assert.IsNotNull(_user.Name);
            Assert.AreEqual("FirstName", _user.Name);
        }

        [TestMethod]
        public void CreateNewUser_WhenLastNameIsEmptyThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _user.LastName = "");
            Assert.AreEqual("LastName cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewUser_WhenLastNameIsNullThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _user.LastName = null);
            Assert.AreEqual("LastName cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewUser_WhenLastNameIsNotNullOrEmptyThenLastNameIsAssigned()
        {
            Assert.IsNotNull(_user.LastName);
            Assert.AreEqual("LastName", _user.LastName);
        }

        [TestMethod]
        public void CreateNewUser_WhenEmailIsEmptyThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _user.Email = "");
            Assert.AreEqual("Email cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewUser_WhenEmailIsNullThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _user.Email = null);
            Assert.AreEqual("Email cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewUser_WhenEmailFormatIsNotValidThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _user.Email = "email.@.com");
            Assert.AreEqual("Email format is invalid", exception.Message);
        }

        [TestMethod]
        public void CreateNewUser_WhenEmailFormatIsValidThenEmailIsAssigned()
        {
            Assert.IsNotNull( _user.Email);
            Assert.AreEqual("email@example.com", _user.Email);
        }

        [TestMethod]
        public void CreateNewUser_WhenPasswordIsEmptyThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _user.Password = "");
            Assert.AreEqual("Password cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewUser_WhenPasswordIsNullThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _user.Password = null);
            Assert.AreEqual("Password cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewUser_WhenPasswordLengthIsNotValidThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _user.Password = "aPasswd");
            Assert.AreEqual("Password must have eight chars or more", exception.Message);
        }

        [TestMethod]
        public void CreateNewUser_WhenPasswordLengthIsValidThenPasswordIsAssigned()
        {
            Assert.IsNotNull(_user.Password);
            Assert.AreEqual("password1234", _user.Password);
        }

        [TestMethod]
        public void CreateNewUser_WhenRoleIsValidThenRoleIsAssigned()
        {
            Assert.IsNotNull(_user.RoleId);
            Assert.AreEqual(_role.Id, _user.RoleId);
        }
    }
}
