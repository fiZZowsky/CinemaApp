using CinemaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Infrastructure.Persistance
{
    public class CinemaAppDbContext : DbContext
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
