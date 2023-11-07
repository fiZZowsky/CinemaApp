using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateCheckoutInDatabase
{
    public class CreateCheckoutInDatabaseCommand : IRequest
    {
        public string SessionId { get; set; }
        public string TicketId { get; set; }

        public CreateCheckoutInDatabaseCommand(string sessionId, string ticketId)
        {
            SessionId = sessionId;
            TicketId = ticketId;
        }
    }
}
