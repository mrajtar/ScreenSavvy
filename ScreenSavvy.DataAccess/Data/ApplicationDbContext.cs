using Microsoft.EntityFrameworkCore;
using ScreenSavvy.Models.Entities;

namespace ScreenSavvy.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<MovieDetails> MovieDetails { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });


            modelBuilder.Entity<Genre>()
                .HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Sci-Fi" },
                new Genre { Id = 3, Name = "Romance" },
                new Genre { Id = 4, Name = "Drama" },
                new Genre { Id = 5, Name = "Fantasy" },
                new Genre { Id = 6, Name = "Animation" },
                new Genre { Id = 7, Name = "Thriller" },
                new Genre { Id = 8, Name = "Comedy" },
                new Genre { Id = 9, Name = "Adventure" },
                new Genre { Id = 10, Name = "Crime" },
                new Genre { Id = 11, Name = "Horror" }
                );
        }
    }
}
