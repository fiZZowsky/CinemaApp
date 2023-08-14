using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAllMoviesDetails
{
    public class GetAllMoviesDetailsQueryHandler : IRequestHandler<GetAllMoviesDetailsQuery, IEnumerable<MovieDetailsDto>>
    {
        private readonly IMovieDetailsRepository _movieDetailsRepository;
        private readonly IMapper _mapper;

        public GetAllMoviesDetailsQueryHandler(IMovieDetailsRepository movieDetailsRepository, IMapper mapper)
        {
            _movieDetailsRepository = movieDetailsRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieDetailsDto>> Handle(GetAllMoviesDetailsQuery request, CancellationToken cancellationToken)
        {
            var movies = await _movieDetailsRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);

            return dtos;
        }
    }
}
