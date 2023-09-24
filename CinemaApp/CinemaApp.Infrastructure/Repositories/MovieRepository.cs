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

        public async Task<IEnumerable<Movie>> GetAll()
            => await _dbContext.Movies.ToListAsync();

        public async Task<MovieShow> GetMovieByEncodedTitle(string encodedTitle)
            => await _dbContext.MovieShows
                .Include(m => m.Movie)
                    .ThenInclude(m => m.AgeRating)
                .Include(h => h.Hall)
                .FirstAsync(m => m.Movie.EncodedTitle == encodedTitle);

        public async Task<bool> IsHallBusy(int hallNumber, DateTime startTime)
        {
            var showsInHall = await _dbContext.MovieShows
                .Include(ms => ms.Movie)
                    .ThenInclude(m => m.AgeRating)
                .Include(ms => ms.Hall)
                .Where(ms => ms.Hall.Number == hallNumber)
                .ToListAsync();

            foreach (var show in showsInHall)
            {
                var endTime = show.StartTime.AddMinutes(show.Movie.Duration + 15);

                if (startTime >= show.StartTime && startTime < endTime)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsMovieExist(string title)
        {
            var movie = await _dbContext.Movies
                .FirstOrDefaultAsync(m => m.Title == title);
            if (movie != null)
                return true;

            return false;
        }
    }
}
