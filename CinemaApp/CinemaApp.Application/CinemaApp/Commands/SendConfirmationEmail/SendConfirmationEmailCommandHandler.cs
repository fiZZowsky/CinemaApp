using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.SendConfirmationEmail
{
    public class SendConfirmationEmailCommandHandler : IRequestHandler<SendConfirmationEmailCommand>
    {
        private readonly IIdentityRepository _identityRepository;

        public SendConfirmationEmailCommandHandler(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }
        public async Task<Unit> Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            await _identityRepository.SendConfirmationEmail(request.Email, request.EmailTemplateText, request.Callback);

            return Unit.Value;
        }
    }
}
