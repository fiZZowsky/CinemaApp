using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;

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
    }
}
