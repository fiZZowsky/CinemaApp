﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        // TO DO: Rozbić na osobne tabele
        public string Genre { get; set; } = default!;
        // -------------------------
        public string Country { get; set; } = default!;
        public string Language { get; set; } = default!;
        public int Duration { get; set; }
        public int AgeRatingId { get; set; }
        public string Description { get; set; } = default!;
        public DateTime ReleaseDate { get; set; }
        public AgeRating AgeRating { get; set; } = default!;

        public List<MovieShow> MovieShows { get; set; } = default!;

        public string EncodedTitle { get; private set; } = default!;

        public void EncodeTitle() => EncodedTitle = Title.ToLower().Replace(" ", "_");
    }
}
