using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class MovieShow
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int HallId { get; set; }
        public DateTime StartTime { get; set; }
        public List<Ticket> Tickets { get; set; } = default!;
        public Movie Movie { get; set; } = default!;
        public Hall Hall { get; set; } = default!;
    }
}
