using AutoMapper;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateTicket
{
    public class TicketCommandHandler : IRequestHandler<CreateTicketCommand>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public TicketCommandHandler(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = _mapper.Map<Domain.Entities.Ticket>(request.TicketDto);

            await _ticketRepository.Create(ticket, request.MovieShowId, request.SeatId);

            return Unit.Value;
        }
    }
}
