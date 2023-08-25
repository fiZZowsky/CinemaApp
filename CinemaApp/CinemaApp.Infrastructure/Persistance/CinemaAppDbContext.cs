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

            modelBuilder.Entity<Domain.Entities.Ticket>()
                .HasOne(t => t.MovieShow)
                .WithMany(ms => ms.Tickets)
                .HasForeignKey(t => t.MovieShowId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Domain.Entities.MovieShow>()
                .HasOne(ms => ms.Movie)
                .WithMany(m => m.MovieShows)
                .HasForeignKey(ms => ms.MovieId);

            modelBuilder.Entity<Domain.Entities.MovieShow>()
                .HasOne(ms => ms.Hall)
                .WithMany(h => h.MoviesList)
                .HasForeignKey(ms => ms.HallId);

            modelBuilder.Entity<Domain.Entities.Hall>()
                .HasMany(h => h.SeatsList)
                .WithOne(s => s.Hall)
                .HasForeignKey(s => s.HallId);
        }
    }
}
