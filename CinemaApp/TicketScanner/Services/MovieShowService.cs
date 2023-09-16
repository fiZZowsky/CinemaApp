using Microsoft.EntityFrameworkCore;
using TicketScanner.Entities;
using TicketScanner.Persistance;

namespace TicketScanner.Services
{
    public class MovieShowService : IMovieShowService
    {
        private readonly TicketScannerDbContext _dbContext;
        public MovieShowService(TicketScannerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<MovieShow>> GetMovieShows()
        {
            var movieShows = await _dbContext.MovieShows
                .Include(m => m.Movie)
                .Include(h => h.Hall)
                .ToListAsync();

            return movieShows;
        }
    }
}
