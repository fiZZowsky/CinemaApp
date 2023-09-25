using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateCheckoutInDatabase
{
    public class CreateCheckoutInDatabaseCommand : IRequest
    {
        public string SessionId { get; set; }
        public Guid TicketId { get; set; }

        public CreateCheckoutInDatabaseCommand(string sessionId, Guid ticketId)
        {
            SessionId = sessionId;
            TicketId = ticketId;
        }
    }
}
