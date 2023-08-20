using CinemaApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Persistance
{
    public class CinemaAppDbContext : IdentityDbContext
    {
        public CinemaAppDbContext(DbContextOptions<CinemaAppDbContext> options) : base(options)
        {

        }

        public DbSet<Domain.Entities.Movie> Movies { get; set; }
        public DbSet<Domain.Entities.Ticket> Tickets { get; set; }
        public DbSet<Domain.Entities.Hall> Halls { get; set; }
        public DbSet<Domain.Entities.Seat> Seats { get; set; }
        public DbSet<Domain.Entities.MovieShow> MovieShows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithMany(s => s.Tickets)
                .HasForeignKey(t => t.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.MovieShow)
                .WithMany(ms => ms.Tickets)
                .HasForeignKey(t => t.MovieShowId)
                .OnDelete(DeleteBehavior.Restrict);           
        }
    }
}
