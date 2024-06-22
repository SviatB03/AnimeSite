using System.ComponentModel.DataAnnotations;

namespace AnimeSite.Models
{
    public class AssignDateViewModel
    {
        public int AnimeId { get; set; }

        public string Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
    }
}
