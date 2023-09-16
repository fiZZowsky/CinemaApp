using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketScanner.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int RowNumber { get; set; }
        public int HallId { get; set; }
        public List<Ticket> Tickets { get; set; }
        public Hall Hall { get; set; }
    }
}
