using AnimeSite.Entity;

namespace AnimeSite.Models
{
    public class AdminAnimeViewModel
    {
        public Anime Anime { get; set; } // Об'єкт аніме для створення або редагування
        public IEnumerable<Genre> Genres { get; set; } // Список категорій (жанрів)
    }
}
