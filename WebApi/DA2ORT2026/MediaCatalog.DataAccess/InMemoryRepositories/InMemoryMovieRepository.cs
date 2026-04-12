
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using System.Data;
using System.Linq.Expressions;

namespace MediaCatalog.DataAccess.InMemoryRepositories
{
    public class InMemoryMovieRepository : IMovieRepository
    {
        private List<Movie> Movies { get; }

        public InMemoryMovieRepository()
        {
            Movies = new List<Movie>();
            LoadDefaultMovies();
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

        public bool Exists(Expression<Func<Movie, bool>> predicate)
        {
            return Movies.AsQueryable().Any(predicate);
        }

        private void LoadDefaultMovies()
        {
            Movies.Add(new Movie(1, "Cast Away", "Robert Zemeckis", new DateTime(2000, 12, 22), 25000000));
        }
    }
}
