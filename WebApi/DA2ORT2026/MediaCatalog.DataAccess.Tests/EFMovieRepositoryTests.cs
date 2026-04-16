
using MediaCatalog.DataAccess.EFRepositories;
using MediaCatalog.Domain;
using MediaCatalog.Services.Interfaces.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MediaCatalog.DataAccess.Tests
{
    [TestClass]
    public class EFMovieRepositoryTests
    {
        private IMovieRepository _movieRepository;
        private AppDbContext _appDbContext;
        private SqliteConnection _connection;
        private Movie _movie;
        private List<Movie> _movies;

        [TestInitialize]
        public void Setup()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            _appDbContext = new AppDbContext(options);
            _appDbContext.Database.EnsureCreated();

            _movieRepository = new EFMovieRepository(_appDbContext);

            _movie = new Movie(1, "aTitle", "aDirector", new DateTime(2000, 12, 12), 30000000);
            _movies = new List<Movie> { _movie };

            _appDbContext.Movies.AddRange(_movies);
            _appDbContext.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _appDbContext.Database.EnsureDeleted();
        }

        [TestMethod]
        public void GetMovies_WhenCalled_ThenAllMoviesAreReturned()
        {
            //arrange
            List<Movie> expectedMovies = _appDbContext.Movies.ToList();

            //act
            List<Movie> retrievedMovies = _movieRepository.GetMovies();

            //assert
            CollectionAssert.AreEqual(expectedMovies, retrievedMovies);
        }

        [TestMethod]
        public void GetMovie_WhenCalled_ThenMovieIsReturned()
        {
            //arrange
            Movie expectedMovie = _appDbContext.Movies.First();

            //act
            Movie? retrievedMovie = _movieRepository.GetMovie(m => m.Id == expectedMovie.Id);

            //assert
            Assert.AreEqual(expectedMovie, retrievedMovie);
        }

        [TestMethod]
        public void AddMovie_WhenCalled_ThenMovieIsAdded()
        {
            //arrange
            Movie movieTwo = new Movie(2, "aTitle2", "aDirector2", new DateTime(2020, 04, 06), 20000000);

            //act
            _movieRepository.AddMovie(movieTwo);

            //assert
            Movie? retrievedMovie = _movieRepository.GetMovie(r => r.Id == movieTwo.Id);
            Assert.IsNotNull(retrievedMovie);
            Assert.AreEqual(movieTwo, retrievedMovie);
        }

        [TestMethod]
        public void UpdateMovie_WhenCalled_ThenMovieIsUpdated()
        {
            //assert
            Movie existentMovie = _appDbContext.Movies.First();
            existentMovie.Director = "Updated Director";

            //act
            _movieRepository.UpdateMovie(existentMovie);

            //assert
            Movie? retrievedMovie = _movieRepository.GetMovie(m => m.Id == existentMovie.Id);
            Assert.IsNotNull(retrievedMovie);
            Assert.AreEqual(existentMovie, retrievedMovie);
        }

        [TestMethod]
        public void DeleteMovie_WhenCalled_ThenMovieIsDeleted()
        {
            //arrange
            Movie existingMovie = _appDbContext.Movies.First();

            //act
            _movieRepository.DeleteMovie(existingMovie);

            //assert
            Movie? retrievedMovie = _movieRepository.GetMovie(m => m.Id == existingMovie.Id);
            Assert.IsNull(retrievedMovie);
        }

        [TestMethod]
        public void Exists_WhenCalledWithExistentEntity_ThenReturnTrue()
        {
            //arrange
            Movie movieTwo = new Movie(2, "aTitle2", "aDirector2", new DateTime(2020, 04, 06), 20000000);
            _appDbContext.Movies.Add(movieTwo);
            _appDbContext.SaveChanges();

            //act
            bool exists = _movieRepository.Exists(predicate: m => m.Title == "aTitle2");

            //assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void Exists_WhenCalledWithNonExistentEntity_ThenReturnFalse()
        {
            //arrange

            //act
            bool exists = _movieRepository.Exists(predicate: m => m.Title == "movieTitle");

            //assert
            Assert.IsFalse(exists);
        }
    }
}
