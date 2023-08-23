using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetTicketByUser
{
    public class GetTicketByUserQuery : IRequest<TicketDto>
    {
        public DateTime PurchaseDate { get; set; }
        public string MovieTitle { get; set; }

        public GetTicketByUserQuery(DateTime purchaseDate, string movieTitle)
        {
            PurchaseDate = purchaseDate;
            MovieTitle = movieTitle;
        }
    }
}
