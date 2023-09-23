using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAllMoviesShows
{
    public class GetAllMoviesShowsQueryHandler : IRequestHandler<GetAllMoviesShowsQuery, IEnumerable<MovieDto>>
    {
        private readonly IMovieShowRepository _movieShowRepository;
        private readonly IMapper _mapper;

        public GetAllMoviesShowsQueryHandler(IMovieShowRepository movieShowRepository, IMapper mapper)
        {
            _movieShowRepository = movieShowRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieDto>> Handle(GetAllMoviesShowsQuery request, CancellationToken cancellationToken)
        {
            var movies = await _movieShowRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<MovieDto>>(movies);

            return dtos;
        }
    }
}
