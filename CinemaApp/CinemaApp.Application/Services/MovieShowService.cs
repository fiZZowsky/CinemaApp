using CinemaApp.Domain.Interfaces;

namespace CinemaApp.Application.Services
{
    public class MovieShowService : IMovieShowService
    {
        private readonly IMovieShowRepository _movieShowRepository;

        public MovieShowService(IMovieShowRepository movieShowRepository)
        {
            _movieShowRepository = movieShowRepository;
        }

        public async Task<Domain.Entities.MovieShow> GetByData(DateTime startTime, int hallNumber)
        {
            var movieShow = await _movieShowRepository.GetByData(startTime, hallNumber);
            return movieShow;
        }
    }
}
