using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetRepertoire
{
    public class GetRepertoireQueryHandler : IRequestHandler<GetRepertoireQuery, IEnumerable<MovieDto>>
    {
        private readonly IMovieShowRepository _movieShowRepository;
        private readonly IMapper _mapper;

        public GetRepertoireQueryHandler(IMovieShowRepository movieShowRepository, IMapper mapper)
        {
            _movieShowRepository = movieShowRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieDto>> Handle(GetRepertoireQuery request, CancellationToken cancellationToken)
        {
            var shows = await _movieShowRepository.GetRepertoire(request?.HallNumber, request?.StartTime);
            var dto = _mapper.Map<IEnumerable<MovieDto>>(shows);

            return dto;
        }
    }
}
