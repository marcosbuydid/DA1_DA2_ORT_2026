using MediaCatalog.Domain;
using MediaCatalog.Services.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MediaCatalog.DataAccess.EFRepositories
{
    public class EFMovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;

        public EFMovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public Movie? GetMovie(Func<Movie, bool> filter)
        {
            return _context.Set<Movie>().FirstOrDefault(filter);
        }

        public List<Movie> GetMovies()
        {
            return _context.Set<Movie>().AsQueryable<Movie>().ToList();
        }

        public void AddMovie(Movie movie)
        {
            _context.Set<Movie>().Add(movie);
            _context.SaveChanges();
        }

        public void UpdateMovie(Movie movie)
        {
            _context.Update(movie);
            _context.Entry(movie).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteMovie(Movie movie)
        {
            _context.Set<Movie>().Remove(movie);
            _context.SaveChanges();
        }

        public bool Exists(Expression<Func<Movie, bool>> predicate)
        {
            return _context.Movies.Any(predicate);
        }
    }
}
