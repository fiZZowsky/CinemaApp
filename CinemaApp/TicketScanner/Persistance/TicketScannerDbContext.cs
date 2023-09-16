using Microsoft.EntityFrameworkCore;
using TicketScanner.Entities;

namespace TicketScanner.Persistance
{
    public class TicketScannerDbContext : DbContext
    {
        public TicketScannerDbContext(DbContextOptions<TicketScannerDbContext> options) : base(options) { }
    
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieShow> MovieShows { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
