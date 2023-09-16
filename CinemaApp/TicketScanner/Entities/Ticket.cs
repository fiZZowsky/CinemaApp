using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketScanner.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int MovieShowId { get; set; }
        public List<Seat> Seats { get; set; }
        public DateTime PurchaseDate { get; set; }
        public byte[] QRCode { get; set; }
        public bool IsScanned { get; set; }
        public string PurchasedById { get; set; }
        public int NormalPriceSeats { get; set; }
        public int ReducedPriceSeats { get; set; }
        public MovieShow MovieShow { get; set; }
    }
}
