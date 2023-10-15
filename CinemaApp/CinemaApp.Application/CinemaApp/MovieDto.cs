namespace CinemaApp.Application.CinemaApp
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = default!;
        public string Genre { get; set; } = default!;
        public string Country { get; set; } = default!;
        public int AgeRatingId { get; set; }
        public string Language { get; set; } = default!;
        public int Duration { get; set; }
        public string Description { get; set; } = default!;
        public DateTime ReleaseDate { get; set; }
        public DateTime StartTime { get; set; }
        public int? HallNumber { get; set; }
        public int NormalTicketPrice { get; set; }
        public int ReducedTicketPrice { get; set; }

        public string? EncodedTitle { get; set; }
    }
}
