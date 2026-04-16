
using MediaCatalog.Api.Controllers;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using MediaCatalog.Services.Models.GenericWrapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MediaCatalog.Api.Tests
{
    [TestClass]
    public class MovieControllerTests
    {
        private Mock<IMovieService> _movieServiceMock;
        private MovieController _movieController;
        private List<MovieDetailDTO> movies;
        MovieCreateDTO movie;
        MovieDetailDTO expectedMovie;
        MovieUpdateDTO movieToUpdate;
        MovieDetailDTO expectedUpdatedMovie;

        [TestInitialize]
        public void Setup()
        {
            _movieServiceMock = new Mock<IMovieService>(MockBehavior.Strict);
            _movieController = new MovieController(_movieServiceMock.Object);

            movies = new List<MovieDetailDTO>()
            {
                new MovieDetailDTO() { Title = "Gladiator 2" },
                new MovieDetailDTO() { Title = "Interstellar" }
            };

            movie = new MovieCreateDTO() { Title = "Train Dreams" };
            expectedMovie = new MovieDetailDTO() { Title = "Train Dreams" };

            movieToUpdate = new MovieUpdateDTO("Christopher Nolan",
                new DateTime(2010, 7, 16));

            expectedUpdatedMovie = new MovieDetailDTO()
            {
                Director = "Christopher Nolan Updated",
                ReleaseDate = new DateTime(2020, 7, 16)
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _movieServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Get_WhenCalled_ThenMoviesAreReturned()
        {
            //arrange
            _movieServiceMock.Setup(ms => ms.GetMovies()).Returns(movies);

            //act
            IActionResult? result = _movieController.Get();

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult.");
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<List<MovieDetailDTO>>;
            Assert.IsNotNull(response);

            var returnedMovies = response.Result;
            Assert.IsNotNull(returnedMovies);
            Assert.AreEqual(2, returnedMovies.Count);

            Assert.AreEqual("Gladiator 2", returnedMovies[0].Title);
            Assert.AreEqual("Interstellar", returnedMovies[1].Title);

            _movieServiceMock.Verify(ms => ms.GetMovies(), Times.Once);
        }

        [TestMethod]
        public void GetByTitle_WhenCalled_ThenMovieIsReturned()
        {
            //arrange
            _movieServiceMock.Setup(ms => ms.GetMovie("Train Dreams")).Returns(expectedMovie);

            //act
            IActionResult? result = _movieController.GetByTitle("Train Dreams");

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult.");
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<MovieDetailDTO>;
            Assert.IsNotNull(response);

            var returnedMovie = response.Result;
            Assert.IsNotNull(returnedMovie);

            Assert.AreEqual("Train Dreams", returnedMovie.Title);

            _movieServiceMock.Verify(ms => ms.GetMovie("Train Dreams"), Times.Once);
        }

        [TestMethod]
        public void Create_WhenCalled_ThenMovieIsCreated()
        {
            //arrange
            _movieServiceMock.Setup(ms => ms.AddMovie(It.IsAny<MovieCreateDTO>()))
                .Returns(expectedMovie);

            //act
            IActionResult? result = _movieController.Create(movie);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<MovieDetailDTO>;
            Assert.IsNotNull(response);

            var createdMovie = response.Result;
            Assert.IsNotNull(createdMovie);

            Assert.AreEqual("Train Dreams", createdMovie.Title);

            _movieServiceMock.Verify(ms => ms.AddMovie(It.IsAny<MovieCreateDTO>()), Times.Once);
        }

        [TestMethod]
        public void Delete_WhenCalled_ThenMovieIsDeleted()
        {
            //arrange
            int movieId = 1;
            _movieServiceMock.Setup(ms => ms.DeleteMovieById(movieId));

            //act
            IActionResult result = _movieController.Delete(movieId);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<string>;
            Assert.IsNotNull(response);

            var message = response.Result;
            Assert.IsNotNull(message);

            Assert.AreEqual("Movie deleted successfully.", message);

            //verify service call
            _movieServiceMock.Verify(ms => ms.DeleteMovieById(movieId), Times.Once);
        }

        [TestMethod]
        public void DeleteByTitle_WhenCalled_ThenMovieIsDeleted()
        {
            //arrange
            _movieServiceMock.Setup(ms => ms.DeleteMovie(movie.Title));

            //act
            IActionResult result = _movieController.DeleteByTitle(movie.Title);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<string>;
            Assert.IsNotNull(response);

            var message = response.Result;
            Assert.IsNotNull(message);

            Assert.AreEqual("Movie deleted successfully.", message);

            // verify service call
            _movieServiceMock.Verify(ms => ms.DeleteMovie(movie.Title), Times.Once);
        }

        [TestMethod]
        public void Update_WhenCalled_ThenMovieIsUpdated()
        {
            //arrange
            int movieId = 1;
            _movieServiceMock.Setup(ms => ms.UpdateMovieById(movieId, movieToUpdate))
                .Returns(expectedUpdatedMovie);

            //act
            IActionResult result = _movieController.Update(movieId, movieToUpdate);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<MovieDetailDTO>;
            Assert.IsNotNull(response);

            var returnedMovie = response.Result;
            Assert.IsNotNull(returnedMovie);

            Assert.AreEqual(expectedUpdatedMovie.Director, returnedMovie.Director);
            Assert.AreEqual(expectedUpdatedMovie.ReleaseDate, returnedMovie.ReleaseDate);

            _movieServiceMock.Verify(ms => ms.UpdateMovieById(movieId, movieToUpdate), Times.Once);
        }

        [TestMethod]
        public void UpdateByTitle_WhenCalled_ThenMovieIsUpdated()
        {
            //arrange
            string movieTitle = "Inception";
            _movieServiceMock.Setup(ms => ms.UpdateMovie(movieTitle, movieToUpdate))
                .Returns(expectedUpdatedMovie);

            //act
            IActionResult result = _movieController.UpdateByTitle(movieTitle, movieToUpdate);

            //assert
            Assert.IsNotNull(result);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var response = okResult.Value as ApiResponse<MovieDetailDTO>;
            Assert.IsNotNull(response);

            var returnedMovie = response.Result;
            Assert.IsNotNull(returnedMovie);

            Assert.AreEqual(expectedUpdatedMovie.Director, returnedMovie.Director);
            Assert.AreEqual(expectedUpdatedMovie.ReleaseDate, returnedMovie.ReleaseDate);

            _movieServiceMock.Verify(ms => ms.UpdateMovie(movieTitle, movieToUpdate), Times.Once);
        }
    }
}
