using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetMovieByEncodedTitle
{
    public class GetMovieByEncodedTitleQueryHandler : IRequestHandler<GetMovieByEncodedTitleQuery, MovieDto>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public GetMovieByEncodedTitleQueryHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<MovieDto> Handle(GetMovieByEncodedTitleQuery request, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetMovieByEncodedTitle(request.EncodedTitle);
            var dto = _mapper.Map<MovieDto>(movie);

            return dto;
        }
    }
}
