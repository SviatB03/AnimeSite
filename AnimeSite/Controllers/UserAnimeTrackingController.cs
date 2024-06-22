using AnimeSite.Models;
using AnimeSite.Repository;
using AnimeSite.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnimeSite.Controllers
{
    public class UserAnimeTrackingController : Controller
    {
        private readonly IUserAnimeTrackingRepository _userAnimeTrackingRepository;
        private readonly IAnimeRepository _animeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IReleaseScheduleRepository _releaseScheduleRepository;

        public UserAnimeTrackingController(IUserAnimeTrackingRepository userAnimeTrackingRepository, IAnimeRepository animeRepository, IUserRepository userRepository, IGenreRepository genreRepository, IReleaseScheduleRepository releaseSchedule)
        {
            _userAnimeTrackingRepository = userAnimeTrackingRepository;
            _animeRepository = animeRepository;
            _userRepository = userRepository;
            _genreRepository = genreRepository;
            _releaseScheduleRepository = releaseSchedule;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var userAnimeTrackings = await _userAnimeTrackingRepository.GetByUserIdAsync(userId.Value);
            var animeViewModels = new List<UserAnimeTrackingViewModel>();

            foreach (var userAnimeTracking in userAnimeTrackings)
            {
                var anime = await _animeRepository.GetByIdAsync(userAnimeTracking.AnimeId);
                if (anime != null)
                {
                    var animeViewModel = new UserAnimeTrackingViewModel
                    {
                        UserAnimeTrackingId = userAnimeTracking.UserAnimeTrackingId,
                        UserId = userAnimeTracking.UserId,
                        AnimeId = userAnimeTracking.AnimeId,
                        AnimeTitle = anime.Title,
                        AnimeDescription = anime.Description,
                        AnimeImagePath = anime.ImagePath
                    };

                    // Отримуємо категорію аніме
                    var genre = await _genreRepository.GetByIdAsync(anime.GenreId);
                    if (genre != null)
                    {
                        animeViewModel.AnimeGenre = genre.Name;
                    }

                    // Отримуємо дату релізу аніме
                    var releaseSchedule = await _releaseScheduleRepository.GetByAnimeIdAsync(anime.AnimeId);
                    if (releaseSchedule != null)
                    {
                        animeViewModel.AnimeReleaseDate = releaseSchedule.ReleaseDate;
                    }

                    animeViewModels.Add(animeViewModel);
                }
            }

            return View(animeViewModels);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userAnimeTrackingRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Handle exception
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
