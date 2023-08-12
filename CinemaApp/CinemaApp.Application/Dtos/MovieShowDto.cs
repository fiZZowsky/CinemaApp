namespace CinemaApp.Application.Dtos
{
    public class MovieShowDto
    {
        public string Title { get; set; } = default!;
        public string Genre { get; set; } = default!;
        public string AgeRating { get; set; } = default!;
        public string Language { get; set; } = default!;
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }
        public int HallNumber { get; set; }
    }
}
