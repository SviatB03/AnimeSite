﻿namespace AnimeSite.Models
{
    public class ReleaseSchedule
    {
        public int ReleaseScheduleId { get; set; }
        public int AnimeId { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
