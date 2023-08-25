namespace CinemaApp.Application.CinemaApp
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; } = default!;
        public string Language { get; set; } = default!;
        public int Duration { get; set; }
        public DateTime StartTime { get; set; }
        public int HallNumber { get; set; }
        public List<int> RowNumber { get; set; } = default!;
        public List<int> SeatNumber { get; set; } = default!;
        public int NormalPriceSeats { get; set; }
        public int ReducedPriceSeats { get; set; }
        public DateTime PurchaseDate { get; set; }
        public byte[] QRCode { get; set; } = default!;
    }
}
