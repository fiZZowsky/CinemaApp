using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;

namespace CinemaApp.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaAppDbContext _dbContext;

        public MovieRepository(CinemaAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(Domain.Entities.Movie movie)
        {
            _dbContext.Add(movie);
            await _dbContext.SaveChangesAsync();
        }
    }
}
