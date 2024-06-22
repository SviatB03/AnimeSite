using AnimeSite.Models;

namespace AnimeSite.Repository.Interfaces
{
    public interface IUserAnimeTrackingRepository
    {
        Task<IEnumerable<UserAnimeTracking>> GetAllAsync();
        Task<UserAnimeTracking> GetByIdAsync(int id);
        Task AddAsync(UserAnimeTracking userAnimeTracking);
        Task UpdateAsync(UserAnimeTracking userAnimeTracking);
        Task DeleteAsync(int id);
    }
}
