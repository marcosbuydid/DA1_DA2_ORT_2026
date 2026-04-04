
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class MovieUpdateDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Director is required.")]
        public string Director { get; set; }

        [Required(ErrorMessage = "ReleaseDate is required.")]
        public DateTime ReleaseDate { get; set; }

        public MovieUpdateDTO() { }

        public MovieUpdateDTO(string title, string director, DateTime releaseDate)
        {
            Title = title;
            Director = director;
            ReleaseDate = releaseDate;
        }
    }
}
