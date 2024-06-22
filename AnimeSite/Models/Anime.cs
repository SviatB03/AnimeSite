namespace AnimeSite.Models
{
    public class Anime
    {
        public int AnimeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int GenreId { get; set; }
    }
}
