using CinemaApp.Domain.Interfaces;

namespace CinemaApp.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public async Task Create(Domain.Entities.Movie movie)
        {
            await _movieRepository.Create(movie);
        }
    }
}
