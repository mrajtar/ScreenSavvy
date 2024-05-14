using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ScreenSavvy.DataAccess.Data;
using ScreenSavvy.DataAccess.Repository.IRepository;
using ScreenSavvy.Models.Entities;
using ScreenSavvy.Models.ViewModels;
using ScreenSavvy.Services.Exceptions;
using ScreenSavvy.Services.Interfaces;

namespace ScreenSavvy.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMoviesRepository _moviesRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger<MoviesService> _logger;
        public MoviesService(IMoviesRepository moviesRepository, IWebHostEnvironment webHostEnvironment, ILogger<MoviesService> logger, IGenreRepository genreRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _moviesRepository = moviesRepository;
            _logger = logger;
            _genreRepository = genreRepository;
        }
        public async Task<MovieDetailsVM> GetMovieAsync(int id)
        {
            try
            {
                var movie = await _moviesRepository.GetAsync(m => m.Id == id, includeProperties: "MovieGenres");
                var movieDetailsVM = new MovieDetailsVM
                {
                    MovieDetails = movie
                };

                return movieDetailsVM;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching movie with ID {id}.");
                throw new ServiceException($"Error fetching movie with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<MovieDetailsVM>> GetAllMoviesAsync()
        {
            try
            {
                var movies = await _moviesRepository.GetAllAsync(includeProperties: "MovieGenres");
                var movieDetailsVM = movies.Select(movie => new MovieDetailsVM
                {
                    MovieDetails = movie
                }).ToList();

                return movieDetailsVM;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all movies.");
                throw new ServiceException("Error retrieving movies.", ex);
            }
        }

        public async Task AddMovieAsync(MovieDetailsVM movieDetailsVM, IFormFile? file)
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
                    movieDetailsVM.MovieDetails.ImagePath = @"\images\movieposters\" + fileName;
                }

                await _moviesRepository.AddAsync(movieDetailsVM.MovieDetails);
                await _moviesRepository.SaveChangesAsync();

                var genres = await _genreRepository.GetAllAsync();
                movieDetailsVM.MovieDetails.MovieGenres = new List<MovieGenre>();

                foreach (var genreId in movieDetailsVM.SelectedGenreIds)
                {
                    var genre = genres.FirstOrDefault(g => g.Id == genreId);
                    if (genre != null)
                    {
                        movieDetailsVM.MovieDetails.MovieGenres.Add(new MovieGenre
                        {
                            MovieId = movieDetailsVM.MovieDetails.Id,
                            GenreId = genre.Id
                        });
                    }
                }

                _moviesRepository.Update(movieDetailsVM.MovieDetails);
                await _moviesRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new movie.");
                throw new ServiceException("Error adding a movie.", ex);
            }
        }

        public async Task UpdateMovieAsync(MovieDetailsVM movieDetailsVM, IFormFile? file)
        {
            try
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string moviePosterPath = Path.Combine(wwwRootPath, @"images\movieposters");

                    if (!string.IsNullOrEmpty(movieDetailsVM.MovieDetails.ImagePath))
                    {
                        var oldImagePath =
                            Path.Combine(wwwRootPath, movieDetailsVM.MovieDetails.ImagePath.TrimStart('\\'));

                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(moviePosterPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    movieDetailsVM.MovieDetails.ImagePath = @"\images\movieposters\" + fileName;
                }
                
                _moviesRepository.Update(movieDetailsVM.MovieDetails);
                await _moviesRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating movie with ID {movieDetailsVM.MovieDetails.Id}.");
                throw new ServiceException($"Error updating movie with ID {movieDetailsVM.MovieDetails.Id}.", ex);
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
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
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
