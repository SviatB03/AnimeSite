using AnimeSite.Entity;
using AnimeSite.Models;
using AnimeSite.Repository;
using AnimeSite.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnimeSite.Controllers
{
    public class AnimeController : Controller
    {
        private readonly IAnimeRepository _animeRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IReleaseScheduleRepository _releaseScheduleRepository;
        private readonly IUserAnimeTrackingRepository _userAnimeTrackingRepository;

        public AnimeController(IAnimeRepository animeRepository, IGenreRepository genreRepository, IReleaseScheduleRepository releaseScheduleRepository, IUserAnimeTrackingRepository userAnimeTrackingRepository)
        {
            _animeRepository = animeRepository;
            _genreRepository = genreRepository;
            _releaseScheduleRepository = releaseScheduleRepository;
            _userAnimeTrackingRepository = userAnimeTrackingRepository;
        }

        public async Task<IActionResult> Index(string searchString, int? genreId, DateTime? startDate, DateTime? endDate)
        {
            var animes = await _animeRepository.GetAllAsync();
            var schedules = await _releaseScheduleRepository.GetAllAsync();

            var query = from anime in animes
                        join schedule in schedules on anime.AnimeId equals schedule.AnimeId
                        select new
                        {
                            Anime = anime,
                            ReleaseDate = schedule.ReleaseDate
                        };

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(q => q.Anime.Title.Contains(searchString));
            }

            if (genreId.HasValue)
            {
                query = query.Where(q => q.Anime.GenreId == genreId);
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(q => q.ReleaseDate >= startDate && q.ReleaseDate <= endDate);
            }

            var genres = await _genreRepository.GetAllAsync();
            var model = new AnimeViewModel
            {
                Animes = query.Select(q => q.Anime).ToList(),
                Genres = genres,
                SearchString = searchString,
                GenreId = genreId,
                StartDate = startDate,
                EndDate = endDate
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToSaved(int animeId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                // Обробка, якщо користувач не авторизований
                return RedirectToAction("Login", "Account");
            }

            // Перевірка, чи це аніме вже є у збережених користувачем
            var existingTracking = await _userAnimeTrackingRepository.GetByUserAndAnimeIdAsync(userId.Value, animeId);
            if (existingTracking != null)
            {
                // Обробка, якщо аніме вже збережене
                TempData["Message"] = "Це аніме вже додане в збережені.";
                return RedirectToAction("Index");
            }

            // Додавання аніме до збережених
            var tracking = new UserAnimeTracking
            {
                UserId = userId.Value,
                AnimeId = animeId
            };

            await _userAnimeTrackingRepository.AddAsync(tracking);
            TempData["Message"] = "Аніме успішно додане в збережене!";
            return RedirectToAction("Index");
        }

    }
}
