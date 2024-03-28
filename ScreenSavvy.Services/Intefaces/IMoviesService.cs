using Microsoft.AspNetCore.Http;
using ScreenSavvy.Models.Entities;

namespace ScreenSavvy.Services.Intefaces
{
    public interface IMoviesService
    {
        Task<IEnumerable<MovieDetails>> GetAllMoviesAsync();
        Task<MovieDetails> GetMovieAsync(int id);
        Task AddMovieAsync(MovieDetails movie, IFormFile? file);
        Task UpdateMovieAsync(MovieDetails movie, IFormFile? file);
        Task DeleteMovieAsync(int id);
    }
}
