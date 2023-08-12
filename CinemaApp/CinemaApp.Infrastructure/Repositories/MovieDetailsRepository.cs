using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Repositories
{
    public class MovieDetailsRepository : IMovieDetailsRepository
    {
        private readonly CinemaAppDbContext _dbContext;

        public MovieDetailsRepository(CinemaAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Domain.Entities.MovieShow movie)
        {
            _dbContext.Add(movie);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovieShow>> GetAll()
        {
            var movieShows = await _dbContext.MovieShows
                .Include(m => m.Movie)
                .Include(h => h.Hall)
                .ToListAsync();
            return movieShows;
        }
    }
}
