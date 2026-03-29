
using MediaCatalog.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaCatalog.Domain.Tests
{
    [TestClass]
    public class SessionTests
    {
        private User _user;
        private Role _role;
        private Session _session;

        [TestInitialize]
        public void Initialize()
        {
            _role = new Role(1, "User");
            _user = new User(1, "FirstName", "LastName", "email@example.com", "password1234", _role);
            _session = new Session(1, "aToken", _user);
        }

        [TestMethod]
        public void CreateNewSession_WhenTokenIsEmptyThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _session.Token = "");
            Assert.AreEqual("Token cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewSession_WhenTokenIsNullThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _session.Token = null);
            Assert.AreEqual("Token cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewSession_WhenTokenIsValidThenTokenIsAssigned()
        {
            Assert.IsNotNull(_session.Token);
            Assert.AreEqual("aToken", _session.Token);
        }

        [TestMethod]
        public void CreateNewSession_WhenUserIsValidThenUserIsAssigned()
        {
            Assert.IsNotNull(_session.User);
            Assert.AreEqual(_user, _session.User);
        }

        [TestMethod]
        public void CreateNewSession_WhenCreatedThenCreatedAtIsAssigned()
        {
            Assert.IsNotNull(_session.CreatedAt);
        }
    }
}
