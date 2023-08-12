using AutoMapper;
using CinemaApp.Application.Dtos;
using CinemaApp.Domain.Interfaces;

namespace CinemaApp.Application.Services
{
    public class MovieDetailsService : IMovieDetailsService
    {
        private readonly IMovieDetailsRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieDetailsService(IMovieDetailsRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task Create(MovieDetailsDto movieDto)
        {
            var movie = _mapper.Map<Domain.Entities.MovieShow>(movieDto);
            movie.Movie.EncodeTitle();

            await _movieRepository.Create(movie);
        }
    }
}
