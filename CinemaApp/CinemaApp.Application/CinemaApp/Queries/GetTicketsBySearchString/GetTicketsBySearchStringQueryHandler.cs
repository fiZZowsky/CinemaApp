using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetTicketsBySearchString
{
    public class GetTicketsBySearchStringQueryHandler : IRequestHandler<GetTicketsBySearchStringQuery, IEnumerable<TicketDto>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public GetTicketsBySearchStringQueryHandler(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper  = mapper;
        }

        public async Task<IEnumerable<TicketDto>> Handle(GetTicketsBySearchStringQuery request, CancellationToken cancellationToken)
        {
            var tickets = await _ticketRepository.GetTicketsBySearchString(request?.Uid);
            var dto = _mapper.Map<IEnumerable<TicketDto>>(tickets);

            return dto;
        }
    }
}
