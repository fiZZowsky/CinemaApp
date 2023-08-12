using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly CinemaAppDbContext _dbContext;

        public TicketRepository(CinemaAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(Domain.Entities.Ticket ticket)
        {
            _dbContext.Add(ticket);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Ticket>> GetAll()
        {
            var tickets = await _dbContext.Tickets
                .Include(s => s.MovieShow)
                    .ThenInclude(m => m.Movie)
                .Include(ticket => ticket.Seat)
                    .ThenInclude(h => h.Hall)
                .ToListAsync();

            return tickets;
        }

    }
}
