using ScreenSavvy.DataAccess.Data;
using ScreenSavvy.DataAccess.Repository.IRepository;
using ScreenSavvy.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSavvy.DataAccess.Repository
{
    public class MoviesRepository : Repository<MovieDetails>, IMoviesRepository
    {
        public MoviesRepository(ApplicationDbContext context) : base(context)
        {
            
        }
    }
}
