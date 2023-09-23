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
                .ToListAsync();

        public async Task Create(Domain.Entities.MovieShow show)
        {
            _dbContext.Add(show);
            await _dbContext.SaveChangesAsync();
        }

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

        public async Task<bool> IsHallBusy(int hallId, DateTime startTime)
        {
            var showsInHall = await _dbContext.MovieShows
                .Include(ms => ms.Movie)
                    .ThenInclude(m => m.AgeRating)
                .Include(ms => ms.Hall)
                .Where(ms => ms.HallId == hallId)
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
    }
}
