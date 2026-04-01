
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;

namespace MediaCatalog.DataAccess.InMemoryRepositories
{
    public class InMemoryMovieRepository : IMovieRepository
    {
        private List<Movie> Movies { get; }

        public InMemoryMovieRepository()
        {
            Movies = new List<Movie>();
        }

        public Movie? GetMovie(Func<Movie, bool> filter)
        {
            return Movies.Where(filter).FirstOrDefault();
        }

        public List<Movie> GetMovies()
        {
            return Movies;
        }

        public void AddMovie(Movie movie)
        {
            Movies.Add(movie);
        }

        public void UpdateMovie(Movie movieToUpdate)
        {
            Movie? movie = Movies.Find(m => m.Title == movieToUpdate.Title);
            var movieToUpdateIndex = Movies.IndexOf(movie);
            movieToUpdate.Budget = movie.Budget;
            Movies[movieToUpdateIndex] = movieToUpdate;
        }

        public void DeleteMovie(Movie movie)
        {
            Movies.Remove(movie);
        }

        public bool Exists(Func<Movie, bool> predicate)
        {
            return Movies.Where(predicate).Any();
        }
    }
}
