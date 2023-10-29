namespace CinemaApp.Application.CinemaApp
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int RateValue { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string CreatedByUserId { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public int MovieId { get; set; }
    }
}
