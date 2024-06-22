using AnimeSite.Entity;

namespace AnimeSite.Repository.Interfaces
{
    public interface IReleaseScheduleRepository
    {
        Task<IEnumerable<ReleaseSchedule>> GetAllAsync();
        Task<ReleaseSchedule> GetByAnimeIdAsync(int animeId);
        Task AddAsync(ReleaseSchedule releaseSchedule);
        Task UpdateAsync(ReleaseSchedule releaseSchedule);
        Task DeleteAsync(int id);
        Task DeleteByAnimeIdAsync(int animeId);
    }

}
