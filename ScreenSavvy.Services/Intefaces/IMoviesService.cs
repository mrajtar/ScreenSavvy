using Microsoft.AspNetCore.Http;
using ScreenSavvy.Models.Entities;

namespace ScreenSavvy.Services.Intefaces
{
    public interface IMoviesService
    {
        Task<IEnumerable<MovieDetails>> GetAllMoviesAsync();
        Task<MovieDetails> GetMoviesAsync(int id);
        Task AddMovieAsync(MovieDetails movie, IFormFile? file);
        Task UpdateMovieAsync(MovieDetails movie);
        Task DeleteMovieAsync(int id);
    }
}
