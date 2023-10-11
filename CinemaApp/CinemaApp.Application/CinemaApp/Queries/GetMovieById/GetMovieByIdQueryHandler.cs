using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetMovieById
{
    public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieDto>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetMovieByIdQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<MovieDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetMovieById(request.Id);
            var dto = _mapper.Map<MovieDto>(movie);

            return dto;
        }
    }
}
