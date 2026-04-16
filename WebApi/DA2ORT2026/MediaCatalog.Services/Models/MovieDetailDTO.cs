
namespace MediaCatalog.Services.Models
{
    public class MovieDetailDTO
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string Director { get; set; }

        public DateTime ReleaseDate { get; set; }

        public long Budget { get; set; }

        public MovieDetailDTO() { }

        public MovieDetailDTO(int? id, string title, string director, DateTime releaseDate, long budget)
        {
            Id = id;
            Title = title;
            Director = director;
            ReleaseDate = releaseDate;
            Budget = budget;
        }

        public MovieDetailDTO(int? id, string title, string director, DateTime releaseDate)
        {
            Id = id;
            Title = title;
            Director = director;
            ReleaseDate = releaseDate;
        }
    }
}
