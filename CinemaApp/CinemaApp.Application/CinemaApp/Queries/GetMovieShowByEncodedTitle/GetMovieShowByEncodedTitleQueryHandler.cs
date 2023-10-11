using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetMovieShowByEncodedTitle
{
    public class GetMovieShowByEncodedTitleQueryHandler : IRequestHandler<GetMovieShowByEncodedTitleQuery, MovieDto>
    {
        private readonly IMovieShowRepository _movieShowRepository;
        private readonly IMapper _mapper;

        public GetMovieShowByEncodedTitleQueryHandler(IMovieShowRepository movieShowRepository, IMapper mapper)
        {
            _movieShowRepository = movieShowRepository;
            _mapper = mapper;
        }

        public async Task<MovieDto> Handle(GetMovieShowByEncodedTitleQuery request, CancellationToken cancellationToken)
        {
            var movie = await _movieShowRepository.GetMovieShowByEncodedTitle(request.EncodedTitle);
            var dto = _mapper.Map<MovieDto>(movie);

            return dto;
        }
    }
}
