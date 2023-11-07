namespace CinemaApp.Application.CinemaApp
{
    public class TicketCheckDto
    {
        public string Uid { get; set; } = default!;
        public string MovieTitle { get; set; } = default!;
        public int MovieDuration { get; set; }
        public DateTime StartTime { get; set; }
        public int HallNumber { get; set; }
        public List<int> RowNumber { get; set; } = default!;
        public List<int> SeatNumber { get; set; } = default!;
        public int NormalPriceSeats { get; set; }
        public int ReducedPriceSeats { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsScanned { get; set; }
    }
}
