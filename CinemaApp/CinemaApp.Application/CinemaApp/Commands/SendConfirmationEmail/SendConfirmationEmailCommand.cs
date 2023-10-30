using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.SendConfirmationEmail
{
    public class SendConfirmationEmailCommand : IRequest
    {
        public string Email { get; set; }
        public string EmailTemplateText { get; set; }
        public string Callback { get; set; }

        public SendConfirmationEmailCommand(string email, string emailTemplateText, string callback)
        {
            Email = email;
            EmailTemplateText = emailTemplateText;
            Callback = callback;
        }
    }
}
