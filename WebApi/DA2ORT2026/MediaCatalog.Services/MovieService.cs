using MediaCatalog.Domain;
using MediaCatalog.Services.Exceptions;
using MediaCatalog.Services.Interfaces;
using MediaCatalog.Services.Interfaces.Repositories;
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
            ValidateReleaseDate(movie.ReleaseDate);

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
            //if (String.IsNullOrWhiteSpace(title))
            //{
            //    throw new ServiceException("Title cannot be null or empty");
            //}

            //Movie? movieToDelete = _movieRepository.GetMovie(m => m.Title.Equals(title, 
            //    StringComparison.OrdinalIgnoreCase));

            //if (movieToDelete == null)
            //{
            //    throw new ResourceNotFoundException("Cannot find a movie with this title");
            //}

            Movie movieToDelete = GetMovieByTitle(title);

            _movieRepository.DeleteMovie(movieToDelete);
        }

        public void DeleteMovieById(int movieId)
        {
            //Movie? movieToDelete = _movieRepository.GetMovie(m => m.Id == movieId);
            //if (movieToDelete == null)
            //{
            //    throw new ResourceNotFoundException("Cannot find a movie with this id");
            //}

            Movie movieToDelete = GetMovieById(movieId);

            _movieRepository.DeleteMovie(movieToDelete);
        }

        public MovieDetailDTO GetMovie(string title)
        {
            //if (String.IsNullOrWhiteSpace(title))
            //{
            //    throw new ServiceException("Title cannot be null or empty");
            //}

            //Movie? movie = _movieRepository.GetMovie(m => m.Title.Equals(title, 
            //    StringComparison.OrdinalIgnoreCase));

            //if (movie == null)
            //{
            //    throw new ResourceNotFoundException("Cannot find a movie with this title");
            //}

            Movie movie = GetMovieByTitle(title);

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

        public MovieDetailDTO UpdateMovie(string title, MovieUpdateDTO movieToUpdate)
        {
            //if (String.IsNullOrWhiteSpace(title))
            //{
            //    throw new ServiceException("Title cannot be null or empty");
            //}

            //Movie? movie = _movieRepository.GetMovie(m => m.Title.Equals(title,
            //    StringComparison.OrdinalIgnoreCase));

            //if (movie == null)
            //{
            //    throw new ResourceNotFoundException("Cannot find the specified movie");
            //}

            Movie movie = GetMovieByTitle(title);

            ValidateReleaseDate(movieToUpdate.ReleaseDate);

            movie.Director = movieToUpdate.Director;
            movie.ReleaseDate = movieToUpdate.ReleaseDate;
            _movieRepository.UpdateMovie(movie);

            return FromEntity(movie);
        }


        public MovieDetailDTO UpdateMovieById(int movieId, MovieUpdateDTO movieToUpdate)
        {
            //Movie? movie = _movieRepository.GetMovie(m => m.Id == movieId);

            //if (movie == null)
            //{
            //    throw new ResourceNotFoundException("Cannot find the specified movie with this id");
            //}

            Movie movie = GetMovieById(movieId);

            ValidateReleaseDate(movieToUpdate.ReleaseDate);

            movie.Director = movieToUpdate.Director;
            movie.ReleaseDate = movieToUpdate.ReleaseDate;
            _movieRepository.UpdateMovie(movie);

            return FromEntity(movie);
        }

        private void ValidateUniqueTitle(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
            {
                throw new ServiceException("Title cannot be null or empty");
            }

            string inputTitle = title.Trim().ToLowerInvariant();

            //hash set for faster lookups if repository has many items
            HashSet<string> movieTitles = new HashSet<string>(_movieRepository.GetMovies()
                .Select(m => m.Title.ToLowerInvariant()));

            if (movieTitles.Contains(inputTitle))
            {
                throw new ConflictException("There’s a movie already defined with that title");
            }
        }

        private void ValidateReleaseDate(DateTime releaseDate)
        {
            if ((releaseDate == DateTime.MinValue) || (releaseDate > DateTime.Now))
            {
                throw new ServiceException("Invalid release date");
            }
        }

        private Movie GetMovieByTitle(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
            {
                throw new ServiceException("Title cannot be null or empty");
            }

            Movie? movie = _movieRepository.GetMovie(m => m.Title.Equals(title,
                StringComparison.OrdinalIgnoreCase));

            if (movie == null)
            {
                throw new ResourceNotFoundException("Cannot find a movie with this title");
            }

            return movie;
        }

        private Movie GetMovieById(int movieId)
        {
            Movie? movie = _movieRepository.GetMovie(m => m.Id == movieId);

            if (movie == null)
            {
                throw new ResourceNotFoundException("Cannot find a movie with this id");
            }

            return movie;
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
