using MediaCatalog.Api.Filters;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
using MediaCatalog.Services.Models.GenericWrapper;
using Microsoft.AspNetCore.Mvc;

namespace MediaCatalog.Api.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult Get()
        {
            List<MovieDetailDTO> movies = _movieService.GetMovies();
            return Ok(new ApiResponse<List<MovieDetailDTO>> { Result = movies });
        }

        [HttpGet("by-title/{title}")]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult GetByTitle(string title)
        {
            MovieDetailDTO movie = _movieService.GetMovie(title);
            return Ok(new ApiResponse<MovieDetailDTO> { Result = movie });
        }

        [HttpPost]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult Create([FromBody] MovieCreateDTO newMovie)
        {
            MovieDetailDTO movie = _movieService.AddMovie(newMovie);
            return Ok(new ApiResponse<MovieDetailDTO> { Result = movie });
        }

        [HttpPut("{movieId}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult Update(int movieId, [FromBody] MovieUpdateDTO movieToUpdate)
        {
            MovieDetailDTO movie = _movieService.UpdateMovieById(movieId, movieToUpdate);
            return Ok(new ApiResponse<MovieDetailDTO> { Result = movie });
        }

        [HttpPut("by-title/{title}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult UpdateByTitle(string title, [FromBody] MovieUpdateDTO movieToUpdate)
        {
            MovieDetailDTO movie = _movieService.UpdateMovie(title, movieToUpdate);
            return Ok(new ApiResponse<MovieDetailDTO> { Result = movie });
        }

        [HttpDelete("by-title/{title}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult DeleteByTitle(string title)
        {
            _movieService.DeleteMovie(title);
            string Message = "Movie deleted successfully.";
            return Ok(new ApiResponse<string> { Result = Message });
        }

        [HttpDelete("{movieId:int}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult Delete(int movieId)
        {
            _movieService.DeleteMovieById(movieId);
            string Message = "Movie deleted successfully.";
            return Ok(new ApiResponse<string> { Result = Message });

        }
    }
}
