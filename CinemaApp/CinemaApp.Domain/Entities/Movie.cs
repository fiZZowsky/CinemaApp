using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        // TO DO: Rozbić na osobne tabele
        public string Genre { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string AgeRating { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        // -------------------------
        public int Duration { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ProductionYear { get; set; }
        public DateTime ReleaseDate { get; set; }

        public List<MovieShow> MovieShows { get; set; } = default!;

        public string EncodedTitle { get; private set; } = default!;

        public void EncodeTitle() => EncodedTitle = Title.ToLower().Replace(" ", "_");
    }
}
