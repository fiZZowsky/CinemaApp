using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.SendRecoveryPasswordEmail
{
    public class SendRecoveryPasswordEmailCommand : IRequest
    {
        public string Email { get; set; }
        public string EmailTemplateText { get; set; }
        public string Callback { get; set; }

        public SendRecoveryPasswordEmailCommand(string email, string emailTemplateText, string callback)
        {
            Email = email;
            EmailTemplateText = emailTemplateText;
            Callback = callback;
        }
    }
}
