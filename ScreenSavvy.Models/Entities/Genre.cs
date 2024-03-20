using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSavvy.Models.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MovieGenre> Genres { get; set; }
    }
}
