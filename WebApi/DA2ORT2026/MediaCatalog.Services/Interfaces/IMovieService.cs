
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services.Interfaces
{
    public interface IMovieService
    {
        List<MovieDTO> GetMovies();
        MovieDTO GetMovie(string title);
        MovieDTO AddMovie(MovieDTO movie);
        void DeleteMovie(string title);
        MovieDTO UpdateMovie(MovieDTO movie);
    }
}
