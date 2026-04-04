using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Models;
using Moq;

namespace MediaCatalog.Services.Tests
{
    [TestClass]
    public class MovieServiceTests
    {
        private Mock<IMovieRepository> _movieRepositoryMock;
        private MovieService _movieService;

        [TestInitialize]
        public void Setup()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>(MockBehavior.Strict);
            _movieService = new MovieService(_movieRepositoryMock.Object);
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
            Movie movie = new Movie(1, "Black Rain", "Ridley Scott", new DateTime(1989, 9, 22), 30000000);

            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns(movie);

            //act
            MovieDetailDTO movieDTO = _movieService.GetMovie(movie.Title);

            //assert
            Assert.AreEqual(movieDTO.Title, movie.Title);
            Assert.AreEqual(movieDTO.Director, movie.Director);
            Assert.AreEqual(movieDTO.ReleaseDate, movie.ReleaseDate);

            _movieRepositoryMock.Verify(r => r.GetMovie(It.IsAny<Func<Movie, bool>>()), Times.Once());
        }

        [TestMethod]
        public void GetMovie_WhenCalledWithUnregisteredMovieThenThrowsException()
        {
            //arrange
            Movie movie = new Movie(1, "Black Rain", "Ridley Scott", new DateTime(1989, 9, 22), 30000000);

            MovieCreateDTO movieDTO = new MovieCreateDTO("Black Rain", "Ridley Scott", new DateTime(1989, 9, 22));

            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns((Movie)null);

            //act
            Action action = () => _movieService.GetMovie(movieDTO.Title);

            //assert
            Assert.ThrowsException<ServiceException>(() => _movieService.GetMovie(movieDTO.Title));
            _movieRepositoryMock.Verify(r => r.Exists(It.IsAny<Func<Movie, bool>>()), Times.Never);
        }

        [TestMethod]
        public void AddMovie_WhenCalled_ThenMovieIsAdded()
        {
            //arrange
            MovieCreateDTO movieDTO = new MovieCreateDTO("Black Rain", "Ridley Scott", new DateTime(1989, 9, 22));

            _movieRepositoryMock.Setup(r => r.GetMovies()).Returns(new List<Movie>());

            _movieRepositoryMock.Setup(r => r.AddMovie(It.IsAny<Movie>()));

            //act
            MovieDetailDTO movieAdded = _movieService.AddMovie(movieDTO);

            //assert
            _movieRepositoryMock.Verify(r => r.Exists(It.IsAny<Func<Movie, bool>>()), Times.Never);

            Assert.AreEqual(movieDTO.Title, movieAdded.Title);
            Assert.AreEqual(movieDTO.Director, movieAdded.Director);
            Assert.AreEqual(movieDTO.ReleaseDate, movieAdded.ReleaseDate);
        }

        [TestMethod]
        public void AddMovie_WhenCalledWithATitleAlreadyInUse_ThenThrowsException()
        {
            //arrange
            MovieCreateDTO movieDTO = new MovieCreateDTO("Black Rain", "Ridley Scott", new DateTime(1989, 9, 22));

            //simulate movie with the same title already exists in repository
            List<Movie> existingMovies = new List<Movie>();
            Movie movie = new Movie(1, "Black Rain", "Ridley Scott", new DateTime(1989, 9, 22), 30000000);
            existingMovies.Add(movie);

            _movieRepositoryMock.Setup(r => r.GetMovies()).Returns(existingMovies);

            //act
            Action action = () => _movieService.AddMovie(movieDTO);

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
            Assert.ThrowsException<ServiceException>(action);

            _movieRepositoryMock.Verify(r => r.DeleteMovie(It.IsAny<Movie>()), Times.Never);
        }

        [TestMethod]
        public void DeleteMovie_WhenCalled_ThenMovieIsDeleted()
        {
            //arrange
            Movie movie = new Movie(2, "Sing Sing", "Greg Kwedar", new DateTime(2024, 07, 12), 2000000);

            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns(movie);

            _movieRepositoryMock.Setup(r => r.DeleteMovie(It.IsAny<Movie>()));

            //act
            _movieService.DeleteMovie(movie.Title);

            //assert
            _movieRepositoryMock.Verify(r => r.DeleteMovie(It.Is<Movie>(m => m.Title == "Sing Sing")), Times.Once);
        }

        [TestMethod]
        public void UpdateMovie_WhenCalledWithUnregisteredMovie_ThenThrowsException()
        {
            //arrange
            MovieUpdateDTO movieDTO = new MovieUpdateDTO("aTitle", "aDirector", new DateTime(2020, 01, 11));

            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns((Movie)null);

            //act
            Action action = () => _movieService.UpdateMovie(movieDTO);

            //assert
            Assert.ThrowsException<ServiceException>(action);

            _movieRepositoryMock.Verify(r => r.UpdateMovie(It.IsAny<Movie>()), Times.Never);
        }

        [TestMethod]
        public void UpdateMovie_WhenCalled_ThenMovieIsUpdated()
        {
            //arrange
            Movie existingMovie = new Movie(92, "oldTitle", "oldDirector", new DateTime(2020, 01, 11), 2000000);

            MovieUpdateDTO movieToUpdate = new MovieUpdateDTO("newTitle", "newDirector", new DateTime(2022, 11, 11));

            _movieRepositoryMock.Setup(r => r.GetMovie(It.IsAny<Func<Movie, bool>>())).Returns(existingMovie);

            _movieRepositoryMock.Setup(r => r.UpdateMovie(It.IsAny<Movie>()));

            //act
            MovieDetailDTO updatedMovie = _movieService.UpdateMovie(movieToUpdate);

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
    }
}
