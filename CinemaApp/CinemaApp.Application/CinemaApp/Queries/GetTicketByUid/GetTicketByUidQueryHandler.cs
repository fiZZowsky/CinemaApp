using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetTicketByUid
{
    public class GetTicketByUidQueryHandler : IRequestHandler<GetTicketByUidQuery, TicketCheckDto>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public GetTicketByUidQueryHandler(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<TicketCheckDto> Handle(GetTicketByUidQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetTicketByUid(request.Uid);
            
            var dto = _mapper.Map<TicketCheckDto>(ticket);

            return dto;
        }
    }
}
