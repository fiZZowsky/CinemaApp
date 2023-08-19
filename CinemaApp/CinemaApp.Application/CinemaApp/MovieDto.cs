namespace CinemaApp.Application.CinemaApp
{
    public class MovieDto
    {
        public string Title { get; set; } = default!;
        public string Genre { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string AgeRating { get; set; } = default!;
        public string Language { get; set; } = default!;
        public int Duration { get; set; }
        public string Description { get; set; } = default!;
        public DateTime ProductionYear { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime StartTime { get; set; }
        public int HallNumber { get; set; }

        public string? EncodedTitle { get; set; }
    }
}
