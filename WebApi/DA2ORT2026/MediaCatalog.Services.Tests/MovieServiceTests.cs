using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Models;
using Moq;
using System.Linq.Expressions;

namespace MediaCatalog.Services.Tests
{
    [TestClass]
    public class MovieServiceTests
    {
        private Mock<IMovieRepository> _movieRepositoryMock;
        private MovieService _movieService;
        private Movie _movie;
        private MovieCreateDTO _movieDTO;
        private List<Movie> _existingMovies;
        private Movie _existingMovie;
        private MovieUpdateDTO _movieToUpdate;

        [TestInitialize]
        public void Setup()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>(MockBehavior.Strict);
            _movieService = new MovieService(_movieRepositoryMock.Object);

            _movie = new Movie(1, "Black Rain", "Ridley Scott", new DateTime(1989, 9, 22), 30000000);
            _movieDTO = new MovieCreateDTO("Black Rain", "Ridley Scott", new DateTime(1989, 9, 22));
            _existingMovies = new List<Movie>();
            _existingMovies.Add(_movie);
            _existingMovie = new Movie(92, "oldTitle", "oldDirector", new DateTime(2020, 01, 11), 2000000);
            _movieToUpdate = new MovieUpdateDTO("newDirector", new DateTime(2022, 11, 11));
        }

        [TestCleanup]
        public void Cleanup()
        {
            _movieRepositoryMock.VerifyAll();
        }

        [TestMethod]
        public void GetMovie_WhenCalledThenMovieIsReturned()
        {
            //arrange
            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns(_movie);

            //act
            MovieDetailDTO movieDTO = _movieService.GetMovie(_movie.Title);

            //assert
            Assert.AreEqual(movieDTO.Title, _movie.Title);
            Assert.AreEqual(movieDTO.Director, _movie.Director);
            Assert.AreEqual(movieDTO.ReleaseDate, _movie.ReleaseDate);

            _movieRepositoryMock.Verify(r => r.GetMovie(It.IsAny<Func<Movie, bool>>()), Times.Once());
        }

        [TestMethod]
        public void GetMovie_WhenCalledWithUnregisteredMovieThenThrowsException()
        {
            //arrange
            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns((Movie)null);

            //act
            Action action = () => _movieService.GetMovie(_movieDTO.Title);

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(() => _movieService.GetMovie(_movieDTO.Title));

            _movieRepositoryMock.Verify(r => r.Exists(It.IsAny<Expression<Func<Movie, bool>>>()), Times.Never);
        }

        [TestMethod]
        public void AddMovie_WhenCalled_ThenMovieIsAdded()
        {
            //arrange
            _movieRepositoryMock.Setup(r => r.GetMovies()).Returns(new List<Movie>());
            _movieRepositoryMock.Setup(r => r.AddMovie(It.IsAny<Movie>()));

            //act
            MovieDetailDTO movieAdded = _movieService.AddMovie(_movieDTO);

            //assert
            Assert.AreEqual(_movieDTO.Title, movieAdded.Title);
            Assert.AreEqual(_movieDTO.Director, movieAdded.Director);
            Assert.AreEqual(_movieDTO.ReleaseDate, movieAdded.ReleaseDate);

            _movieRepositoryMock.Verify(r => r.Exists(It.IsAny<Expression<Func<Movie, bool>>>()), Times.Never);
        }

        [TestMethod]
        public void AddMovie_WhenCalledWithATitleAlreadyInUse_ThenThrowsException()
        {
            //arrange
            //simulate movie with the same title already exists in repository
            _movieRepositoryMock.Setup(r => r.GetMovies()).Returns(_existingMovies);

            //act
            Action action = () => _movieService.AddMovie(_movieDTO);

            //assert
            Assert.ThrowsException<ConflictException>(action);
            _movieRepositoryMock.Verify(r => r.AddMovie(It.IsAny<Movie>()), Times.Never);
        }

        [TestMethod]
        public void AddMovie_WhenCalledWithAInvalidReleaseDate_ThenThrowsException()
        {
            //arrange
            _movieDTO.ReleaseDate = DateTime.Today.AddYears(10);

            _movieRepositoryMock.Setup(r => r.GetMovies()).Returns(new List<Movie>());

            //act
            Action action = () => _movieService.AddMovie(_movieDTO);

            //assert
            Assert.ThrowsException<ServiceException>(action);

            _movieRepositoryMock.Verify(r => r.AddMovie(It.IsAny<Movie>()), Times.Never);
        }

        [TestMethod]
        public void GetMovies_WhenCalled_ThenMoviesAreReturned()
        {
            //arrange
            List<Movie> movies = new List<Movie>()
                {
                    new Movie(1, "Black Rain", "Ridley Scott", new DateTime(1989, 9, 22), 30000000),
                    new Movie(2, "Sing Sing", "Greg Kwedar", new DateTime(2024, 07, 12), 2000000)
                };

            _movieRepositoryMock.Setup(r => r.GetMovies()).Returns(movies);

            //act
            List<MovieDetailDTO> result = _movieService.GetMovies();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Black Rain", result[0].Title);
            Assert.AreEqual("Sing Sing", result[1].Title);

            _movieRepositoryMock.Verify(r => r.GetMovies(), Times.Once);
        }

        [TestMethod]
        public void DeleteMovie_WhenCalledWithUnregisteredTitle_ThenThrowsException()
        {
            //arrange
            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns((Movie)null);

            //act
            Action action = () => _movieService.DeleteMovie("aMovieTitle");

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(action);

            _movieRepositoryMock.Verify(r => r.DeleteMovie(It.IsAny<Movie>()), Times.Never);
        }

        [TestMethod]
        public void DeleteMovie_WhenCalled_ThenMovieIsDeleted()
        {
            //arrange
            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns(_movie);
            _movieRepositoryMock.Setup(r => r.DeleteMovie(It.IsAny<Movie>()));

            //act
            _movieService.DeleteMovie(_movie.Title);

            //assert
            _movieRepositoryMock.Verify(r => r.DeleteMovie(It.Is<Movie>(m => m.Title == "Black Rain")), Times.Once);
        }

        [TestMethod]
        public void DeleteMovieById_WhenCalledWithInvalidId_ThenThrowsException()
        {
            //arrange
            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns((Movie)null);

            //act
            Action action = () => _movieService.DeleteMovieById(100);

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(action);

            _movieRepositoryMock.Verify(r => r.DeleteMovie(It.IsAny<Movie>()), Times.Never);
        }

        public void DeleteMovieById_WhenCalledWithValidId_ThenMovieIsDeleted()
        {
            //arrange
            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns(_movie);
            _movieRepositoryMock.Setup(r => r.DeleteMovie(It.IsAny<Movie>()));

            //act
            _movieService.DeleteMovieById(_movie.Id);

            //assert
            _movieRepositoryMock.Verify(r => r.DeleteMovie(It.Is<Movie>(m => m.Title == "Black Rain")), Times.Once);
        }

        [TestMethod]
        public void UpdateMovie_WhenCalledWithUnregisteredMovie_ThenThrowsException()
        {
            //arrange
            MovieUpdateDTO movieDTO = new MovieUpdateDTO("aDirector", new DateTime(2020, 01, 11));

            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns((Movie)null);

            //act
            Action action = () => _movieService.UpdateMovie("aTitle", movieDTO);

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(action);

            _movieRepositoryMock.Verify(r => r.UpdateMovie(It.IsAny<Movie>()), Times.Never);
        }

        [TestMethod]
        public void UpdateMovie_WhenCalled_ThenMovieIsUpdated()
        {
            //arrange
            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns(_existingMovie);
            _movieRepositoryMock.Setup(r => r.UpdateMovie(It.IsAny<Movie>()));

            //act
            MovieDetailDTO updatedMovie = _movieService.UpdateMovie(_existingMovie.Title, _movieToUpdate);

            //assert
            _movieRepositoryMock.Verify(
                r => r.UpdateMovie(It.Is<Movie>(m =>
                    m.Title == updatedMovie.Title &&
                    m.Director == updatedMovie.Director &&
                    m.ReleaseDate == updatedMovie.ReleaseDate
                )), Times.Once);

            Assert.AreEqual(updatedMovie.Title, updatedMovie.Title);
            Assert.AreEqual(updatedMovie.Director, updatedMovie.Director);
            Assert.AreEqual(updatedMovie.ReleaseDate, updatedMovie.ReleaseDate);
        }

        [TestMethod]
        public void UpdateMovie_WhenCalledWithValidId_ThenMovieIsUpdated()
        {
            //arrange
            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns(_existingMovie);
            _movieRepositoryMock.Setup(r => r.UpdateMovie(It.IsAny<Movie>()));

            //act
            MovieDetailDTO updatedMovie = _movieService.UpdateMovieById(92, _movieToUpdate);

            //assert
            _movieRepositoryMock.Verify(
                r => r.UpdateMovie(It.Is<Movie>(m =>
                    m.Title == updatedMovie.Title &&
                    m.Director == updatedMovie.Director &&
                    m.ReleaseDate == updatedMovie.ReleaseDate
                )), Times.Once);

            Assert.AreEqual(updatedMovie.Title, updatedMovie.Title);
            Assert.AreEqual(updatedMovie.Director, updatedMovie.Director);
            Assert.AreEqual(updatedMovie.ReleaseDate, updatedMovie.ReleaseDate);
        }

        [TestMethod]
        public void UpdateMovieById_WhenCalledWithNotValidId_ThenThrowsException()
        {
            //arrange
            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns((Movie)null);

            //act
            Action action = () => _movieService.UpdateMovieById(24, _movieToUpdate);

            //assert
            Assert.ThrowsException<ResourceNotFoundException>(action);

            _movieRepositoryMock.Verify(r => r.UpdateMovie(It.IsAny<Movie>()), Times.Never);
        }

        [TestMethod]
        public void UpdateMovie_WhenCalledWithNotValidReleaseDate_ThenThrowsException()
        {
            //arrange
            MovieUpdateDTO movieDTO = new MovieUpdateDTO("aDirector", DateTime.Today.AddYears(10));

            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns(_existingMovie);

            //act
            Action action = () => _movieService.UpdateMovie("aTitle", movieDTO);

            //assert
            Assert.ThrowsException<ServiceException>(action);

            _movieRepositoryMock.Verify(r => r.UpdateMovie(It.IsAny<Movie>()), Times.Never);
        }

        [TestMethod]
        public void UpdateMovieById_WhenCalledWithNotValidReleaseDate_ThenThrowsException()
        {
            //arrange
            MovieUpdateDTO movieDTO = new MovieUpdateDTO("aDirector", DateTime.Today.AddYears(10));

            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns(_existingMovie);

            //act
            Action action = () => _movieService.UpdateMovieById(92, movieDTO);

            //assert
            Assert.ThrowsException<ServiceException>(action);

            _movieRepositoryMock.Verify(r => r.UpdateMovie(It.IsAny<Movie>()), Times.Never);
        }
    }
}
