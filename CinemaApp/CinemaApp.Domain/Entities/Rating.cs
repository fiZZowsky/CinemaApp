using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int RateValue { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public int MovieId { get; set; }
        public IdentityUser User { get; set; } = default!;
        public Movie Movie { get; set; } = default!;
    }
}
