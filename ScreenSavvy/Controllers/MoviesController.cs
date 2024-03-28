using Microsoft.AspNetCore.Mvc;
using ScreenSavvy.Models.Entities;
using ScreenSavvy.Services.Intefaces;

namespace ScreenSavvy.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _moviesService;
        public MoviesController(IMoviesService moviesService)
        {
            _moviesService = moviesService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _moviesService.GetAllMoviesAsync());
        }
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _moviesService.GetMovieAsync(id);
            if (movie == null) 
            {
            return NotFound();
            }
            return View(movie);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieDetails movieDetails, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                await _moviesService.AddMovieAsync(movieDetails, file);
                return RedirectToAction(nameof(Index));
            }
            return View(movieDetails);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var movieDetails = await _moviesService.GetMovieAsync(id);
            if (movieDetails == null)
            {
                return NotFound();
            }
            return View(movieDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieDetails movieDetails, IFormFile? file)
        {
            if (id != movieDetails.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _moviesService.UpdateMovieAsync(movieDetails, file);
                return RedirectToAction(nameof(Index));
            }
            return View(movieDetails);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var movieDetails = await _moviesService.GetMovieAsync(id);
            if (movieDetails == null)
            {
                return NotFound();
            }
            return View(movieDetails);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeletePOST(int id)
        {
            await _moviesService.DeleteMovieAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
