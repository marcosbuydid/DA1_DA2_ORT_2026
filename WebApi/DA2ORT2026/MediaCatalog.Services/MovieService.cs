
using MediaCatalog.DataAccess.Interfaces;
using MediaCatalog.Domain;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Models;

namespace MediaCatalog.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public MovieDetailDTO AddMovie(MovieCreateDTO movie)
        {
            ValidateUniqueTitle(movie.Title);
            _movieRepository.AddMovie(ToEntity(movie));
            return new MovieDetailDTO()
            {
                Title = movie.Title,
                Director = movie.Director,
                ReleaseDate = movie.ReleaseDate
            };
        }

        public void DeleteMovie(string title)
        {
            Movie? movieToDelete = _movieRepository.GetMovie(m => m.Title == title);
            if (movieToDelete == null)
            {
                throw new ResourceNotFoundException("Cannot find a movie with this title");
            }

            _movieRepository.DeleteMovie(movieToDelete);
        }

        public void DeleteMovieById(int? movieId)
        {
            Movie? movieToDelete = _movieRepository.GetMovie(m => m.Id == movieId);
            if (movieToDelete == null)
            {
                throw new ResourceNotFoundException("Cannot find a movie with this id");
            }

            _movieRepository.DeleteMovie(movieToDelete);
        }

        public MovieDetailDTO GetMovie(string title)
        {
            Movie? movie = _movieRepository.GetMovie(m => m.Title == title);
            if (movie == null)
            {
                throw new ResourceNotFoundException("Cannot find a movie with this title");
            }

            return FromEntity(movie);
        }

        public List<MovieDetailDTO> GetMovies()
        {
            List<MovieDetailDTO> moviesDTO = new List<MovieDetailDTO>();

            foreach (var movie in _movieRepository.GetMovies())
            {
                moviesDTO.Add(FromEntity(movie));
            }

            return moviesDTO;
        }

        public MovieDetailDTO UpdateMovie(MovieUpdateDTO movieToUpdate)
        {
            Movie? movie = _movieRepository.GetMovie(m => m.Title == movieToUpdate.Title);
            if (movie == null)
            {
                throw new ResourceNotFoundException("Cannot find the specified movie");
            }

            movie.Title = movieToUpdate.Title;
            movie.Director = movieToUpdate.Director;
            movie.ReleaseDate = movieToUpdate.ReleaseDate;
            _movieRepository.UpdateMovie(movie);

            return FromEntity(movie);
        }


        public MovieDetailDTO UpdateMovieById(int? movieId, MovieUpdateDTO movieToUpdate)
        {
            Movie? movie = _movieRepository.GetMovie(m => m.Id == movieId);
            if (movie == null)
            {
                throw new ResourceNotFoundException("Cannot find the specified movie with this id");
            }

            movie.Title = movieToUpdate.Title;
            movie.Director = movieToUpdate.Director;
            movie.ReleaseDate = movieToUpdate.ReleaseDate;
            _movieRepository.UpdateMovie(movie);

            return FromEntity(movie);
        }

        private void ValidateUniqueTitle(string title)
        {
            string inputTitle = title.Trim().ToLowerInvariant();
            foreach (var movie in _movieRepository.GetMovies())
            {
                string retrievedTitle = movie.Title.Trim().ToLowerInvariant();
                if (retrievedTitle == inputTitle)
                {
                    throw new ConflictException("There`s a movie already defined with that title");
                }
            }
        }

        private static Movie ToEntity(MovieCreateDTO movieDTO)
        {
            return new Movie()
            {
                Title = movieDTO.Title,
                Director = movieDTO.Director,
                ReleaseDate = movieDTO.ReleaseDate,
                Budget = movieDTO.Budget
            };
        }

        private static MovieDetailDTO FromEntity(Movie movie)
        {
            return new MovieDetailDTO()
            {
                Id = movie.Id,
                Title = movie.Title,
                Director = movie.Director,
                ReleaseDate = movie.ReleaseDate
            };
        }
    }
}
