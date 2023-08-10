using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;

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
    }
}
