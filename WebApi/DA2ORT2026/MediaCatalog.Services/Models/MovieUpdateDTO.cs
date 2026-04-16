
using System.ComponentModel.DataAnnotations;

namespace MediaCatalog.Services.Models
{
    public class MovieUpdateDTO
    {
        [Required(ErrorMessage = "Director is required.")]
        public string Director { get; set; }

        [Required(ErrorMessage = "ReleaseDate is required.")]
        public DateTime ReleaseDate { get; set; }

        public MovieUpdateDTO() { }

        public MovieUpdateDTO(string director, DateTime releaseDate)
        {
            Director = director;
            ReleaseDate = releaseDate;
        }
    }
}
