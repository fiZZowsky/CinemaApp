using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly CinemaAppDbContext _dbContext;

        public SeatRepository(CinemaAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Domain.Entities.Seat>> GetByData(int hallNumber, List<int> rowNumber, List<int> seatNumber)
        {
            var seats = await _dbContext.Seats
                .Include(s => s.Hall)
                .Where(s => s.Hall.Number == hallNumber && rowNumber.Contains(s.RowNumber) && seatNumber.Contains(s.Number))
                .ToListAsync();

            return seats;
        }
    }
}
