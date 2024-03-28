using System.ComponentModel.DataAnnotations;

namespace ScreenSavvy.Models.Entities
{
    public class MovieDetails
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public string Title { get; set; }
        public string Director { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int ReleaseYear { get; set; }
        public string ImagePath { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
