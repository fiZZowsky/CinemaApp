using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaAppDbContext _dbContext;

        public MovieRepository(CinemaAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
            => await _dbContext.SaveChangesAsync();

        public async Task Create(Domain.Entities.Movie movie)
        {
            _dbContext.Add(movie);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovieShow>> GetAll()
            => await _dbContext.MovieShows
                .Include(m => m.Movie)
                    .ThenInclude(m => m.AgeRating)
                .Include(h => h.Hall)
                .ToListAsync();

        public async Task<MovieShow> GetMovieByEncodedTitle(string encodedTitle)
            => await _dbContext.MovieShows
                .Include(m => m.Movie)
                    .ThenInclude(m => m.AgeRating)
                .Include(h => h.Hall)
                .FirstAsync(m => m.Movie.EncodedTitle == encodedTitle);
    }
}
