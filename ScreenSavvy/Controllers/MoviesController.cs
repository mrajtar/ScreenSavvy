using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScreenSavvy.DataAccess.Data;
using ScreenSavvy.Models.ViewModels;
using ScreenSavvy.Services.Interfaces;

namespace ScreenSavvy.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _moviesService;
        private readonly ApplicationDbContext _context;
        public MoviesController(IMoviesService moviesService, ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Create()
        {
            var movieDetailsVM = new MovieDetailsVM
            {
                Genres = await _context.Genres.ToListAsync()
            };
            return View(movieDetailsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieDetailsVM movie, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                await _moviesService.AddMovieAsync(movie, file);
                return RedirectToAction("Index", "Home");
            }
            return View(movie);
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
        public async Task<IActionResult> Edit(int id, MovieDetailsVM movie, IFormFile? file)
        {
            if (id != movie.MovieDetails.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _moviesService.UpdateMovieAsync(movie, file);
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _moviesService.GetMovieAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
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
