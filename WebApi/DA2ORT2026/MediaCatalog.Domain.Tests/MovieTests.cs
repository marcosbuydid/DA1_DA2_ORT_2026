
using MediaCatalog.Domain.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaCatalog.Domain.Tests
{
    [TestClass]
    public class MovieTests
    {
        private Movie _movie;

        [TestInitialize]
        public void Initialize()
        {
            _movie = new Movie(1,"Gladiator 2", "Ridley Scott", new DateTime(2024, 11, 22), 250000000);
        }

        [TestMethod]
        public void CreateNewMovie_WhenTitleIsEmptyThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _movie.Title = "");
            Assert.AreEqual("Title cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewMovie_WhenTitleIsNullThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _movie.Title = null);
            Assert.AreEqual("Title cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewMovie_WhenTitleIsNotNullOrEmptyThenTitleIsAssigned()
        {
            Assert.IsNotNull(_movie.Title);
            Assert.AreEqual("Gladiator 2", _movie.Title);
        }

        [TestMethod]
        public void CreateNewMovie_WhenDirectorIsEmptyThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _movie.Director = "");
            Assert.AreEqual("Director cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewMovie_WhenDirectorIsNullThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _movie.Director = null);
            Assert.AreEqual("Director cannot be null or empty", exception.Message);
        }

        [TestMethod]
        public void CreateNewMovie_WhenDirectorIsNotNullOrEmptyThenDirectorIsAssigned()
        {
            Assert.IsNotNull(_movie.Director);
            Assert.AreEqual("Ridley Scott", _movie.Director);
        }

        [TestMethod]
        public void CreateNewMovie_WhenBudgetIsNegativeThenThrowsException()
        {
            Exception exception = Assert.ThrowsException<DomainException>(() => _movie.Budget = -10000000);
            Assert.AreEqual("Budget must be a positive number", exception.Message);
        }

        [TestMethod]
        public void CreateNewMovie_WhenBudgetIsPositiveThenBudgetIsAssigned()
        {
            Assert.IsNotNull(_movie.Budget);
            Assert.AreEqual(250000000, _movie.Budget);
        }
    }
}
