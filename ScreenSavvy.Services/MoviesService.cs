using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using ScreenSavvy.DataAccess.Repository.IRepository;
using ScreenSavvy.Models.Entities;
using ScreenSavvy.Services.Exceptions;
using ScreenSavvy.Services.Intefaces;

namespace ScreenSavvy.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMoviesRepository _moviesRepository;
        private readonly ILogger<MoviesService> _logger;
        public MoviesService(IMoviesRepository moviesRepository, IWebHostEnvironment webHostEnvironment, ILogger<MoviesService> logger)
        {
            _webHostEnvironment = webHostEnvironment;
            _moviesRepository = moviesRepository;
            _logger = logger;
        }
        public async Task<MovieDetails> GetMovieAsync(int id)
        {
            try
            {
                var movie = _moviesRepository.GetAsync(m => m.Id == id);
                return await movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching movie with ID {id}.");
                throw new ServiceException($"Error fetching movie with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<MovieDetails>> GetAllMoviesAsync()
        {
            try
            {
                return await _moviesRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all movies.");
                throw new ServiceException("Error retrieving movies.", ex);
            }
        }

        public async Task AddMovieAsync(MovieDetails movie, IFormFile? file)
        {
            try
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string moviePosterPath = Path.Combine(wwwRootPath, @"images\movieposters");

                    using (var fileStream = new FileStream(Path.Combine(moviePosterPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    movie.ImagePath = @"\images\movieposters\" + fileName;
                }
                await _moviesRepository.AddAsync(movie);
                await _moviesRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new movie.");
                throw new ServiceException("Error adding a movie.", ex);
            }
        }

        public async Task UpdateMovieAsync(MovieDetails movie, IFormFile? file)
        {
            try
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string moviePosterPath = Path.Combine(wwwRootPath, @"images\movieposters");

                    if (!string.IsNullOrEmpty(movie.ImagePath))
                    {
                        var oldImagePath =
                            Path.Combine(wwwRootPath, movie.ImagePath.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(moviePosterPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    movie.ImagePath = @"\images\movieposters\" + fileName;
                }
                _moviesRepository.Update(movie);
                await _moviesRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating movie with ID {movie.Id}.");
                throw new ServiceException($"Error updating movie with ID {movie.Id}.", ex);
            }
        }

        public async Task DeleteMovieAsync(int id)
        {
            try
            {
                var movie = await _moviesRepository.GetAsync(m => m.Id == id);
                if (movie != null)
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                        movie.ImagePath.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    _moviesRepository.Delete(movie);
                    await _moviesRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting movie with ID {id}.");
                throw new ServiceException($"Error deleting movie with ID {id}.", ex);
            }
        }
    }
}
