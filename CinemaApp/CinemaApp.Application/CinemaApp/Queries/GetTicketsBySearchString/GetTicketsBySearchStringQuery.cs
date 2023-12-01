using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetTicketsBySearchString
{
    public class GetTicketsBySearchStringQuery : IRequest<IEnumerable<TicketDto>>
    {
        public string? Uid { get; set; }

        public GetTicketsBySearchStringQuery(string? uid)
        {
            if (uid != null) Uid = uid;
        }
    }
}
