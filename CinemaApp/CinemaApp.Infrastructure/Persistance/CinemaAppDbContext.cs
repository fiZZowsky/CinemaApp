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
        public DbSet<Domain.Entities.AgeRating> AgeRatings { get; set; }
        public DbSet<Domain.Entities.Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Domain.Entities.Ticket>()
                .HasOne(t => t.MovieShow)
                .WithMany(ms => ms.Tickets)
                .HasForeignKey(t => t.MovieShowId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Domain.Entities.Ticket>()
                .HasMany(t => t.Seats)
                .WithMany(s => s.Tickets);

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

            modelBuilder.Entity<Domain.Entities.Movie>()
                .HasOne(m => m.AgeRating)
                .WithMany(ar => ar.Movies)
                .HasForeignKey(m => m.AgeRatingId);

            modelBuilder.Entity<Domain.Entities.Payment>()
                .HasOne(p => p.Ticket)
                .WithOne(t => t.Payment)
                .HasForeignKey<Domain.Entities.Payment>(p => p.TicketId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
