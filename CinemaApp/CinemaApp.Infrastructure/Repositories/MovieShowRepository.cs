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
            => await _dbContext.MovieShows
                .Include(m => m.Movie)
                    .ThenInclude(m => m.AgeRating)
                .Include(h => h.Hall)
                .OrderBy(ms => ms.StartTime)
                .ToListAsync();

        public async Task Create(Domain.Entities.MovieShow show)
        {
            _dbContext.Add(show);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Domain.Entities.MovieShow> GetMovieShowByEncodedTitle(string encodedTitle)
            => await _dbContext.MovieShows
                .Include(m => m.Movie)
                    .ThenInclude(m => m.AgeRating)
                .Include(h => h.Hall)
                .FirstAsync(m => m.Movie.EncodedTitle == encodedTitle);

        public async Task<Domain.Entities.MovieShow> GetByData(DateTime startTime, int hallNumber)
        {
            var movieShow = await _dbContext.MovieShows
                    .Include(m => m.Movie)
                        .ThenInclude(m => m.AgeRating)
                    .Include(h => h.Hall)
                    .FirstOrDefaultAsync(st =>
                        st.StartTime.Year == startTime.Year &&
                        st.StartTime.Month == startTime.Month &&
                        st.StartTime.Day == startTime.Day &&
                        st.StartTime.Hour == startTime.Hour &&
                        st.StartTime.Minute == startTime.Minute &&
                        st.Hall.Number == hallNumber);

            return movieShow;
        }

        public async Task<bool> IsHallBusy(int hallNumber, DateTime startTime, string movieTitle)
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

                if (startTime >= show.StartTime && startTime < endTime && show.Movie.Title != movieTitle)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> IsMoviePremiered(int movieId, DateTime startTime)
        {
            var movie = await _dbContext.Movies
                .FirstAsync(m => m.Id == movieId);
            return startTime.Date >= movie.ReleaseDate.Date;
        }

        public async Task<IEnumerable<MovieShow>> GetRepertoire(List<int>? hallNumber, DateTime? startTime, string? searchString)
        {
            var query = _dbContext.MovieShows
                .Include(ms => ms.Movie)
                .ThenInclude(m => m.AgeRating)
                .Include(ms => ms.Hall)
                .OrderBy(ms => ms.StartTime)
                .AsQueryable();

            if (hallNumber != null && hallNumber.Any())
            {
                query = query.Where(ms => hallNumber.Contains(ms.Hall.Number));
            }

            if (startTime.HasValue)
            {
                query = query.Where(ms => ms.StartTime.Date == startTime.Value.Date);
            }

            if (searchString != null)
            {
                query = query.Where(ms => ms.Movie.Title.Contains(searchString) || ms.Movie.Genre.Contains(searchString));
            }

            return await query.ToListAsync();
        }

        public async Task Commit()
            => await _dbContext.SaveChangesAsync();
    }
}
