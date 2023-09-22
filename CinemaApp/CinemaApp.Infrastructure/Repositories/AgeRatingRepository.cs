using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Repositories
{
    public class AgeRatingRepository : IAgeRatingRepository
    {
        private readonly CinemaAppDbContext _dbContext;

        public AgeRatingRepository(CinemaAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AgeRating> GetAgeRatingById(int ageRatingId)
            => await _dbContext.AgeRatings
            .Where(ar => ar.Id == ageRatingId)
            .FirstOrDefaultAsync();

        public async Task<IEnumerable<Domain.Entities.AgeRating>> GetAgeRatings()
            => await _dbContext.AgeRatings.ToListAsync();
    }
}
