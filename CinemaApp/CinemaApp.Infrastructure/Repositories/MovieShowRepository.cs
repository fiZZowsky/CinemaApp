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

        public async Task Create(Domain.Entities.MovieShow show)
        {
            _dbContext.Add(show);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Domain.Entities.MovieShow> GetByData(DateTime startTime, int hallNumber)
        {
            var movieShow = await _dbContext.MovieShows
                    .Include(m => m.Movie)
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
    }
}
