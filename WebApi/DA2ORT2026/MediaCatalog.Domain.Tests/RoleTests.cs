
using MediaCatalog.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaCatalog.Domain.Tests
{
    [TestClass]
    public class RoleTests
    {
        private Role _role;

        [TestInitialize]
        public void Initialize()
        {
            _role = new Role(1, "Administrator");
        }


        [TestMethod]
        public void CreateNewRole_WhenNameIsEmptyThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _role.Name = "");
            Assert.AreEqual("Name cannot be null or empty", exception.Message);
        }


        [TestMethod]
        public void CreateNewRole_WhenNameIsNullThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _role.Name = null);
            Assert.AreEqual("Name cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewRole_WhenNameIsNotNullOrEmptyThenNameIsAssigned()
        {
            Assert.IsNotNull(_role.Name);
            Assert.AreEqual("Administrator", _role.Name);
        }
    }
}
