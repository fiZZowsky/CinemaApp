using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.EditTicket
{
    public class EditTicketCommand : IRequest
    {
        public string Uid { get; set; }

        public EditTicketCommand(string uid)
        {
            Uid = uid;
        }
    }
}
