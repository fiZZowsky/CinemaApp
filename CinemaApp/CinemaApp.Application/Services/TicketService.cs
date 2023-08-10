using CinemaApp.Domain.Interfaces;

namespace CinemaApp.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        public async Task Create(Domain.Entities.Ticket ticket)
        {
            await _ticketRepository.Create(ticket);
        }
    }
}
