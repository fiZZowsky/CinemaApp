using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class Ticket
    {
        [Key]
        public required string Uid { get; set; }
        public int MovieShowId { get; set; } = default!;
        public List<Seat> Seats { get; set; } = new();
        public DateTime PurchaseDate { get; set; }
        public byte[] QRCode { get; set; } = default!;
        public bool IsScanned { get; set; }
        public string PurchasedById { get; set; } = default!;
        public int NormalPriceSeats { get; set; }
        public int ReducedPriceSeats { get; set; }
        public IdentityUser PurchasedBy { get; set; } = default!;
        public MovieShow MovieShow { get; set; } = default!;
        public Payment Payment { get; set; } = default!;
    }
}
