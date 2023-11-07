using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetTicketByUid
{
    public class GetTicketByUidQuery : IRequest<TicketCheckDto>
    {
        public string Uid { get; set; }

        public GetTicketByUidQuery(string uid)
        {
            Uid = uid;
        }
    }
}
