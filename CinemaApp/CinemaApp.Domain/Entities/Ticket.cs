using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int MovieShowId { get; set; }
        public int SeatId { get; set; }
        // TO DO: Replace the type with a separate table extending the tickets table
        public string Type { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public byte[]? QRCode { get; set; }
        public bool IsScanned { get; set; }

        public MovieShow MovieShow { get; set; } = default!;
        public Seat Seat { get; set; } = default!;
    }
}
