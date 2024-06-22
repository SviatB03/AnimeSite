using AnimeSite.Models;

namespace AnimeSite.Repository.Interfaces
{
    public interface IReleaseScheduleRepository
    {
        IEnumerable<ReleaseSchedule> GetAll();
        ReleaseSchedule GetById(int id);
        void Add(ReleaseSchedule releaseSchedule);
        void Update(ReleaseSchedule releaseSchedule);
        void Delete(int id);
    }
}
