using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.EditTicket
{
    public class EditTicketCommandHandler : IRequestHandler<EditTicketCommand>
    {
        private readonly ITicketRepository _ticketRepository;

        public EditTicketCommandHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Unit> Handle(EditTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetTicketByGuid(request.Guid);

            ticket.IsScanned = true;

            await _ticketRepository.Commit();

            return Unit.Value;
        }
    }
}
