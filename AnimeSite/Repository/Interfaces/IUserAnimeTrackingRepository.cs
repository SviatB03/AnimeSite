using AnimeSite.Models;

namespace AnimeSite.Repository.Interfaces
{
    public interface IUserAnimeTrackingRepository
    {
        IEnumerable<UserAnimeTracking> GetAll();
        UserAnimeTracking GetById(int id);
        void Add(UserAnimeTracking userAnimeTracking);
        void Update(UserAnimeTracking userAnimeTracking);
        void Delete(int id);
    }
}
