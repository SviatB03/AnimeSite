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

        public AnimeController(
            IAnimeRepository animeRepository,
            IGenreRepository genreRepository,
            IReleaseScheduleRepository releaseScheduleRepository,
            IUserAnimeTrackingRepository userAnimeTrackingRepository)
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

        // Адміністраторські методи

        public async Task<IActionResult> AdminIndex()
        {
            var animes = await _animeRepository.GetAllAsync();
            return View(animes);
        }

        [HttpGet]
        public async Task<IActionResult> AdminCreate()
        {
            var genres = await _genreRepository.GetAllAsync();
            var viewModel = new AdminAnimeViewModel
            {
                Anime = new Anime(),
                Genres = genres
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCreate(AdminAnimeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await _animeRepository.AddAsync(viewModel.Anime);
                return RedirectToAction(nameof(AdminIndex));
            }

            viewModel.Genres = await _genreRepository.GetAllAsync();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AdminEdit(int id)
        {
            var anime = await _animeRepository.GetByIdAsync(id);
            if (anime == null)
            {
                return NotFound();
            }

            var genres = await _genreRepository.GetAllAsync();
            var viewModel = new AdminAnimeViewModel
            {
                Anime = anime,
                Genres = genres
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEdit(int id, AdminAnimeViewModel viewModel)
        {
            if (id != viewModel.Anime.AnimeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _animeRepository.UpdateAsync(viewModel.Anime);
                return RedirectToAction(nameof(AdminIndex));
            }

            viewModel.Genres = await _genreRepository.GetAllAsync();
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AdminDelete(int id)
        {
            var anime = await _animeRepository.GetByIdAsync(id);
            if (anime == null)
            {
                return NotFound();
            }
            return View(anime);
        }

        [HttpPost, ActionName("AdminDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDeleteConfirmed(int id)
        {
            await _animeRepository.DeleteAsync(id);
            return RedirectToAction(nameof(AdminIndex));
        }

        [HttpGet]
        public async Task<IActionResult> AdminAssignDate(int id)
        {
            var anime = await _animeRepository.GetByIdAsync(id);
            if (anime == null)
            {
                return NotFound();
            }

            var viewModel = new AssignDateViewModel
            {
                AnimeId = anime.AnimeId,
                Title = anime.Title
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminAssignDate(AssignDateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var releaseSchedule = new ReleaseSchedule
                {
                    AnimeId = viewModel.AnimeId,
                    ReleaseDate = viewModel.ReleaseDate
                };

                await _releaseScheduleRepository.AddAsync(releaseSchedule);
                return RedirectToAction(nameof(AdminIndex));
            }

            return View(viewModel);
        }
    }
}
