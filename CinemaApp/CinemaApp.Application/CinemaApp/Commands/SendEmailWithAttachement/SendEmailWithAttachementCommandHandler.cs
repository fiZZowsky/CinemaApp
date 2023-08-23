using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.SendEmailWithAttachement
{
    public class SendEmailWithAttachementCommandHandler : IRequestHandler<SendEmailWithAttachementCommand>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserContext _userContext;

        public SendEmailWithAttachementCommandHandler(ITicketRepository ticketRepository, IUserContext userContext)
        {
            _ticketRepository = ticketRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(SendEmailWithAttachementCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
            {
                return Unit.Value;
            }

            await _ticketRepository.SendEmailWithAttachement(currentUser.Email, request.EmailTemplateText, request.Attachment);

            return Unit.Value;
        }
    }
}
