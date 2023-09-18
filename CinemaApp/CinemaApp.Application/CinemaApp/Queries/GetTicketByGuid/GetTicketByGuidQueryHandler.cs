using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetTicketByGuid
{
    public class GetTicketByGuidQueryHandler : IRequestHandler<GetTicketByGuidQuery, TicketCheckDto>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public GetTicketByGuidQueryHandler(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<TicketCheckDto> Handle(GetTicketByGuidQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetTicketByGuid(request.Guid);
            var ticketDto = _mapper.Map<TicketCheckDto>(ticket);

            return ticketDto;
        }
    }
}
