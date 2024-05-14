using Microsoft.AspNetCore.Http;
using ScreenSavvy.Models.ViewModels;

namespace ScreenSavvy.Services.Interfaces
{
    public interface IMoviesService
    {
        Task<IEnumerable<MovieDetailsVM>> GetAllMoviesAsync();
        Task<MovieDetailsVM> GetMovieAsync(int id);
        Task AddMovieAsync(MovieDetailsVM movie, IFormFile? file);
        Task UpdateMovieAsync(MovieDetailsVM movie, IFormFile? file);
        Task DeleteMovieAsync(int id);
    }
}
