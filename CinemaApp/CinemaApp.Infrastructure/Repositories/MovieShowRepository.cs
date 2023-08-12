using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Repositories
{
    public class MovieShowRepository : IMovieShowRepository
    {
        private readonly CinemaAppDbContext _dbContext;

        public MovieShowRepository(CinemaAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<MovieShow>> GetAll()
            => await _dbContext.MovieShows.ToListAsync();
    }
}
