using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class PriceList
    {
        public int Id { get; set; }
        public int NormalTicketPrice { get; set; }
        public int ReducedTicketPrice { get; set; }
    }
}
