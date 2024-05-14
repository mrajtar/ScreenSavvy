using ScreenSavvy.Models.Entities;

namespace ScreenSavvy.Models.ViewModels
{
    public class MovieDetailsVM
    {
        public MovieDetails MovieDetails { get; set; }
        public List<int> SelectedGenreIds { get; set; } = new List<int>();
        public IEnumerable<Genre> Genres { get; set; }
    }
}
