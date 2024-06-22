using AnimeSite.Entity;
using AnimeSite.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnimeSite.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IGenreRepository _genreRepository;

        public CategoryController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IActionResult> Index()
        {
            var genres = await _genreRepository.GetAllAsync();
            return View(genres);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                await _genreRepository.AddAsync(genre);
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Genre genre)
        {
            if (id != genre.GenreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _genreRepository.UpdateAsync(genre);
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _genreRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
