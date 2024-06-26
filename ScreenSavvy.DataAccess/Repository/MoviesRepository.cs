﻿using ScreenSavvy.DataAccess.Data;
using ScreenSavvy.DataAccess.Repository.IRepository;
using ScreenSavvy.Models.Entities;

namespace ScreenSavvy.DataAccess.Repository
{
    public class MoviesRepository : Repository<MovieDetails>, IMoviesRepository
    {
        public MoviesRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
