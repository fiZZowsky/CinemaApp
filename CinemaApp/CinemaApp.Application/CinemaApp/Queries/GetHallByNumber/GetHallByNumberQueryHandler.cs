using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetHallByNumber
{
    public class GetHallByNumberQueryHandler : IRequestHandler<GetHallByNumberQuery, HallDto>
    {
        private readonly IHallRepository _hallRepository;
        private readonly IMapper _mapper;

        public GetHallByNumberQueryHandler(IHallRepository hallRepository, IMapper mapper)
        {
            _hallRepository = hallRepository;
            _mapper = mapper;
        }

        public async Task<HallDto> Handle(GetHallByNumberQuery request, CancellationToken cancellationToken)
        {
            var hall = await _hallRepository.GetHallByNumber(request.Number);
            var hallDto = _mapper.Map<HallDto>(hall);
            return hallDto;
        }
    }
}
