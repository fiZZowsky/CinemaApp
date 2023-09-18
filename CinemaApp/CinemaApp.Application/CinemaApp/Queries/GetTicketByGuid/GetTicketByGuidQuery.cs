using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetTicketByGuid
{
    public class GetTicketByGuidQuery : IRequest<TicketCheckDto>
    {
        public Guid Guid { get; set; }

        public GetTicketByGuidQuery(Guid guid)
        {
            Guid = guid;
        }
    }
}
