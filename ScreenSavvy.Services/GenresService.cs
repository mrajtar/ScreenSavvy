using Microsoft.Extensions.Logging;
using ScreenSavvy.DataAccess.Repository.IRepository;
using ScreenSavvy.Models.Entities;
using ScreenSavvy.Models.ViewModels;
using ScreenSavvy.Services.Exceptions;
using ScreenSavvy.Services.Interfaces;

namespace ScreenSavvy.Services
{
    public class GenresService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly ILogger<GenresService> _logger;

        public GenresService(IGenreRepository genreRepository, ILogger<GenresService> logger)
        {
            _genreRepository = genreRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            try
            {
                var genres = _genreRepository.GetAllAsync();
                return await genres;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all genres.");
                throw new ServiceException("Error retrieving genres.", ex);
            }
        }

        public async Task<Genre> GetGenreAsync(int id)
        {
            try
            {
                var genres = _genreRepository.GetAsync(g =>  g.Id == id);
                return await genres;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching genre with ID {id}.");
                throw new ServiceException($"Error fetching genre with ID {id}.", ex);
            }
        }

        public async Task AddGenreAsync(Genre genre)
        {
            try
            {
                await _genreRepository.AddAsync(genre);
                await _genreRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new genre.");
                throw new ServiceException("Error adding a genre.", ex);
            }
        }

        public async Task UpdateGenreAsync(Genre genre)
        {
            try
            {
                _genreRepository.Update(genre);
                await _genreRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating genre with ID {genre.Id}.");
                throw new ServiceException($"Error updating genre with ID {genre.Id}.", ex);
            }
        }

        public async Task DeleteGenreAsync(int id)
        {
            try
            {
                var genre = await _genreRepository.GetAsync(g => g.Id == id);
                if (genre != null)
                {
                    _genreRepository.Delete(genre);
                    await _genreRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting genre with ID {id}.");
                throw new ServiceException($"Error deleting genre with ID {id}.", ex);
            }
        }
    }
}
