namespace ScreenSavvy.Models.Entities
{
    public class MovieGenre
    {
        public int MovieId { get; set; }
        public MovieDetails MovieDetails { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
