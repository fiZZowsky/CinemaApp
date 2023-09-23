using AutoMapper;
using CinemaApp.Application.CinemaApp.Queries.GetAllHalls.GetAllHalls;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAllHalls
{
    public class GetAllHallsQueryHandler : IRequestHandler<GetAllHallsQuery, IEnumerable<HallDto>>
    {
        private readonly IHallRepository _hallRepository;
        private readonly IMapper _mapper;

        public GetAllHallsQueryHandler(IHallRepository hallRepository, IMapper mapper)
        {
            _hallRepository = hallRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HallDto>> Handle(GetAllHallsQuery request, CancellationToken cancellationToken)
        {
            var halls = await _hallRepository.GetAllHalls();
            var dtos = _mapper.Map<IEnumerable<HallDto>>(halls);
            return dtos;
        }
    }
}
