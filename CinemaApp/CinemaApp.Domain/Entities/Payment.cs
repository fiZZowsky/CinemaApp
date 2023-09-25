using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string SessionId { get; set; } = default!;
        public Guid TicketId { get; set; }
        public string PurchasedById { get; set; } = default!;
        public DateTime PurchaseDate { get; set; }
        public Ticket Ticket { get; set; } = default!;
        public IdentityUser PurchasedBy { get; set; } = default!;
    }
}
