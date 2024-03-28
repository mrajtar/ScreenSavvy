﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ScreenSavvy.DataAccess.Repository.IRepository;
using ScreenSavvy.Models.Entities;
using ScreenSavvy.Services.Intefaces;

namespace ScreenSavvy.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMoviesRepository _moviesRepository;
        public MoviesService(IMoviesRepository moviesRepository, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _moviesRepository = moviesRepository;
        }
        public async Task<MovieDetails> GetMovieAsync(int id)
        {
            var movie = _moviesRepository.GetAsync(m => m.Id == id);
            return await movie;
        }

        public async Task<IEnumerable<MovieDetails>> GetAllMoviesAsync()
        {
            return await _moviesRepository.GetAllAsync();
        }

        public async Task AddMovieAsync(MovieDetails movie, IFormFile? file)
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

        public async Task UpdateMovieAsync(MovieDetails movie, IFormFile? file)
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

        public async Task DeleteMovieAsync(int id)
        {
            var movie = await _moviesRepository.GetAsync(m => m.Id == id);
            if (movie != null)
            {
                var oldImagePath = Path.Combine (_webHostEnvironment.WebRootPath,
                    movie.ImagePath.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                _moviesRepository.Delete(movie);
                await _moviesRepository.SaveChangesAsync();
            }
        }
    }
}
