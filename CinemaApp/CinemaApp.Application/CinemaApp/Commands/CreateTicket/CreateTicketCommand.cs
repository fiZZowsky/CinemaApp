using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateTicket
{
    public class CreateTicketCommand : TicketDto, IRequest
    {
        public TicketDto TicketDto { get; set; }
        public int MovieShowId { get; set; }
        public List<Domain.Entities.Seat> Seats { get; set; }

        public CreateTicketCommand(TicketDto ticketDto, int movieShowId, List<Domain.Entities.Seat> seats)
        {
            TicketDto = ticketDto;
            MovieShowId = movieShowId;
            Seats = seats;
        }
    }
}
