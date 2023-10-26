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

        public async Task<IEnumerable<Domain.Entities.Movie>> GetAll()
            => await _dbContext.Movies.ToListAsync();

        public async Task<bool> IsMovieExist(string title)
        {
            var movie = await _dbContext.Movies
                .FirstOrDefaultAsync(m => m.Title == title);
            if (movie != null)
                return true;

            return false;
        }

        public async Task<IEnumerable<Domain.Entities.Movie>> GetUpcomingMovies()
            => await _dbContext.Movies
                    .Include(m => m.AgeRating)
                    .Where(m => m.ReleaseDate > DateTime.UtcNow.Date)
                    .ToListAsync();

        public async Task<Domain.Entities.Movie> GetMovieByEncodedTitle(string encodedTitle)
            => await _dbContext.Movies
                .Include(m => m.AgeRating)
                .Where(m => m.EncodedTitle == encodedTitle)
                .FirstAsync();

        public async Task<Domain.Entities.Movie> GetMovieById(int id)
            => await _dbContext.Movies
            .Include(m => m.AgeRating)
            .Where(m => m.Id == id)
            .FirstAsync();
    }
}
