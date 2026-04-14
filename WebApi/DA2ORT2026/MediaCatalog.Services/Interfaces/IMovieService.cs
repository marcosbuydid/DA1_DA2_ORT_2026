
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services.Interfaces
{
    public interface IMovieService
    {
        List<MovieDetailDTO> GetMovies();
        MovieDetailDTO GetMovie(string title);
        MovieDetailDTO AddMovie(MovieCreateDTO movie);
        void DeleteMovie(string title);
        void DeleteMovieById(int movieId);
        MovieDetailDTO UpdateMovie(string title, MovieUpdateDTO movie);
        MovieDetailDTO UpdateMovieById(int movieId, MovieUpdateDTO movie);
    }
}
