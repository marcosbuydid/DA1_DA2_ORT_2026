using MediaCatalog.Api.Filters;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;
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
            return Ok(new { movies });
        }

        [HttpGet("by-title/{title}")]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult GetByTitle(string title)
        {
            MovieDetailDTO movie = _movieService.GetMovie(title);
            return Ok(new { movie });
        }

        [HttpPost]
        [AuthorizationFilter("Administrator,User")]
        public IActionResult Create([FromBody] MovieCreateDTO newMovie)
        {
            MovieDetailDTO movie = _movieService.AddMovie(newMovie);
            return Ok(new { movie });
        }

        [HttpPut("{movieId}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult Update(int movieId, [FromBody] MovieUpdateDTO movieToUpdate)
        {
            MovieDetailDTO movie = _movieService.UpdateMovieById(movieId, movieToUpdate);
            return Ok(new { movie });
        }

        [HttpPut("by-title/{title}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult UpdateByTitle(string title, [FromBody] MovieUpdateDTO movieToUpdate)
        {
            MovieDetailDTO movie = _movieService.UpdateMovie(title, movieToUpdate);
            return Ok(new { movie });
        }

        [HttpDelete("by-title/{title}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult DeleteByTitle(string title)
        {
            _movieService.DeleteMovie(title);
            return Ok(new { Message = "Movie deleted successfully." });
        }

        [HttpDelete("{movieId:int}")]
        [AuthorizationFilter("Administrator")]
        public IActionResult Delete(int movieId)
        {
            _movieService.DeleteMovieById(movieId);
            return Ok(new { Message = "Movie deleted successfully." });
        }
    }
}
