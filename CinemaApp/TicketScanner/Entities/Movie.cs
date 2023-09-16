using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketScanner.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Country { get; set; }
        public string AgeRating { get; set; }
        public string Language { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<MovieShow> MovieShows { get; set; }
    }
}
