using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.EditTicket
{
    public class EditTicketCommand : IRequest
    {
        public Guid Guid { get; set; }

        public EditTicketCommand(Guid guid)
        {
            Guid = guid;
        }
    }
}
