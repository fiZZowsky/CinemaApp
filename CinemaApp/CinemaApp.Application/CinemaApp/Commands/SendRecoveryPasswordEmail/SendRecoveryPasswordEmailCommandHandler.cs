using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.SendRecoveryPasswordEmail
{
    public class SendRecoveryPasswordEmailCommandHandler : IRequestHandler<SendRecoveryPasswordEmailCommand>
    {
        private readonly IIdentityRepository _identityRepository;

        public SendRecoveryPasswordEmailCommandHandler(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<Unit> Handle(SendRecoveryPasswordEmailCommand request, CancellationToken cancellationToken)
        {
            await _identityRepository.SendEmailWithRecoveryPassword(request.Email, request.EmailTemplateText, request.Callback);

            return Unit.Value;
        }
    }
}