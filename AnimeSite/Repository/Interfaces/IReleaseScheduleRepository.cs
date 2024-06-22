using AnimeSite.Models;

namespace AnimeSite.Repository.Interfaces
{
    public interface IReleaseScheduleRepository
    {
        Task<IEnumerable<ReleaseSchedule>> GetAllAsync();
        Task<ReleaseSchedule> GetByIdAsync(int id);
        Task AddAsync(ReleaseSchedule releaseSchedule);
        Task UpdateAsync(ReleaseSchedule releaseSchedule);
        Task DeleteAsync(int id);
    }
}
