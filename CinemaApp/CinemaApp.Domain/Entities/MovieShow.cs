using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Domain.Entities
{
    public class MovieShow
    {
        public int Id { get; set; }
        [Required]
        public int MovieId { get; set; }
        [Required]
        public int HallId { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy HH:mm}")]
        public DateTime StartTime { get; set; }
        public bool IsActive { get; set; }
        public List<Ticket> Tickets { get; set; } = default!;
        public Movie Movie { get; set; } = default!;
        public Hall Hall { get; set; } = default!;
    }
}
