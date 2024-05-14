using Microsoft.AspNetCore.Mvc;
using ScreenSavvy.Models.Entities;
using ScreenSavvy.Services.Interfaces;

namespace ScreenSavvy.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;
        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _genreService.GetAllGenresAsync());
        }
        public async Task<IActionResult> GetMovie(int id)
        {
            var genre = await _genreService.GetGenreAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genre genre)
        {
            await _genreService.AddGenreAsync(genre);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var genre = await _genreService.GetGenreAsync(id);
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
            if (id != genre.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _genreService.UpdateGenreAsync(genre);
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _genreService.GetGenreAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int id)
        {
            await _genreService.DeleteGenreAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
