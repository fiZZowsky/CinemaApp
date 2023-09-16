using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketScanner.Entities
{
    public class Hall
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int NumberOfRows { get; set; }
        public int PlacesInARow { get; set; }
        public List<Seat> SeatsList { get; set; }
        public List<MovieShow> MoviesList { get; set; }
    }
}
