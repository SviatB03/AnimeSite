using AnimeSite.Entity;

namespace AnimeSite.Models
{
    public class AdminAnimeViewModel
    {
        public Anime Anime { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IFormFile Image { get; set; } 
    }
}
