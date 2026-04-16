
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class MovieCreateDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Director is required.")]
        public string Director { get; set; }

        [Required(ErrorMessage = "ReleaseDate is required.")]
        public DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "Budget is required.")]
        public long Budget { get; set; }

        public MovieCreateDTO() { }

        public MovieCreateDTO(string title, string director, DateTime releaseDate, long budget)
        {
            Title = title;
            Director = director;
            ReleaseDate = releaseDate;
            Budget = budget;
        }

        public MovieCreateDTO(string title, string director, DateTime releaseDate)
        {
            Title = title;
            Director = director;
            ReleaseDate = releaseDate;
        }
    }
}
