using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketScanner.Entities
{
    public class MovieShow
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int HallId { get; set; }
        public DateTime StartTime { get; set; }
        public List<Ticket> Tickets { get; set; }
        public Movie Movie { get; set; }
        public Hall Hall { get; set; }
    }
}
