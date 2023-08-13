namespace CinemaApp.Application.Dtos
{
    public class TicketDto
    {
        public string MovieTitle { get; set; } = default!;
        public string Language { get; set; } = default!;
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }
        public int HallNumber { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        public string Type { get; set; } = default!;
        public byte[]? QRCode { get; set; }
    }
}
