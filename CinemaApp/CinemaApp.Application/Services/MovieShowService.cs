using AutoMapper;
using CinemaApp.Application.Dtos;
using CinemaApp.Domain.Interfaces;

namespace CinemaApp.Application.Services
{
    public class MovieShowService : IMovieShowService
    {
        private readonly IMovieShowRepository _movieShowRepository;
        private readonly IMapper _mapper;

        public MovieShowService(IMovieShowRepository movieShowRepository, IMapper mapper)
        {
            _movieShowRepository = movieShowRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieShowDto>> GetAll()
        {
            var movies = await _movieShowRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<MovieShowDto>>(movies);

            return dtos;
        }
    }
}
