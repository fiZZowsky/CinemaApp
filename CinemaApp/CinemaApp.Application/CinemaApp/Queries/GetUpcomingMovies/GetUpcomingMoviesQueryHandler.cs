using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetUpcomingMovies
{
    public class GetUpcomingMoviesQueryHandler : IRequestHandler<GetUpcomingMoviesQuery, IEnumerable<MovieDto>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetUpcomingMoviesQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieDto>> Handle(GetUpcomingMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _movieRepository.GetUpcomingMovies();
            var dto = _mapper.Map<IEnumerable<MovieDto>>(movies);

            return dto;
        }
    }
}
