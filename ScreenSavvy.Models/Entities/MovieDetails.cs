using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSavvy.Models.Entities
{
    public class MovieDetails
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Director { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
    }
}
