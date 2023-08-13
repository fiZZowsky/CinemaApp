using AutoMapper;
using CinemaApp.Application.Dtos;
using CinemaApp.Domain.Interfaces;

namespace CinemaApp.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public TicketService(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task Create(TicketDto ticketDto, int movieShowId, int seatId)
        {
            var ticket = _mapper.Map<Domain.Entities.Ticket>(ticketDto);
            ticket.MovieShowId = movieShowId;
            ticket.SeatId = seatId;
            await _ticketRepository.Create(ticket);
        }

        public async Task<IEnumerable<TicketDto>> GetAll()
        {
            var tickets = await _ticketRepository.GetAll();
            var dtos = _mapper.Map<IEnumerable<TicketDto>>(tickets);

            return dtos;
        }
    }
}
