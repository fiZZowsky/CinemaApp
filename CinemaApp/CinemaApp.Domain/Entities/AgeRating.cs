using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class AgeRating
    {
        public int Id { get; set; }
        public string MinimumAge { get; set; } = string.Empty;
        public List<Movie> Movies { get; set; }
    }
}
