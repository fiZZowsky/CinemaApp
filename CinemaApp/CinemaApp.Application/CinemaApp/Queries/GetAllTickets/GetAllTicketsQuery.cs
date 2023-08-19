using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAllTickets
{
    public class GetAllTicketsQuery : IRequest<IEnumerable<TicketDto>>
    {
    }
}
