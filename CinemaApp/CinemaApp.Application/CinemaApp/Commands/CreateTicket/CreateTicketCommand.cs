using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateTicket
{
    public class CreateTicketCommand : TicketDto, IRequest
    {
        public TicketDto TicketDto { get; set; }
        public int MovieShowId { get; set; }
        public int SeatId { get; set; }

        public CreateTicketCommand(TicketDto ticketDto, int movieShowId, int seatId)
        {
            TicketDto = ticketDto;
            MovieShowId = movieShowId;
            SeatId = seatId;
        }
    }
}
