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
        public async Task<Domain.Entities.Seat> GetByData(int hallNumber, int rowNumber, int seatNumber)
        {
            var seat = await _dbContext.Seats
                .Include(s => s.Hall)
                .FirstOrDefaultAsync(s => s.Hall.Number == hallNumber && s.RowNumber == rowNumber && s.Number == seatNumber);

            return seat;
        }
    }
}
