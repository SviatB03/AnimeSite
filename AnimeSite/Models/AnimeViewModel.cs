using AnimeSite.Entity;

namespace AnimeSite.Models
{
    public class AnimeViewModel
    {
        public IEnumerable<Anime> Animes { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<ReleaseSchedule> ReleaseSchedules { get; set; } 
        public string SearchString { get; set; }
        public int? GenreId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

}
