using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly CinemaAppDbContext _dbContext;

        public RatingRepository(CinemaAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Domain.Entities.Rating rating)
        {
            _dbContext.Add(rating);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsByMovieId(int movieId)
            => await _dbContext.Ratings
            .Where(r => r.MovieId == movieId)
            .ToListAsync();
    }
}
