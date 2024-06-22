using AnimeSite.Models;

namespace AnimeSite.Repository.Interfaces
{
    public interface IAnimeRepository
    {
        IEnumerable<Anime> GetAll();
        Anime GetById(int id);
        void Add(Anime anime);
        void Update(Anime anime);
        void Delete(int id);
    }
}
