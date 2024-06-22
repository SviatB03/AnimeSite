namespace AnimeSite.Models
{
    public class UserAnimeTrackingViewModel
    {
        public int UserAnimeTrackingId { get; set; }
        public int UserId { get; set; }
        public int AnimeId { get; set; }

        //  властивості для даних аніме
        public string AnimeTitle { get; set; }
        public string AnimeDescription { get; set; }
        public string AnimeImagePath { get; set; }
        public string AnimeGenre { get; set; }
        public DateTime AnimeReleaseDate { get; set; }
    }
}
