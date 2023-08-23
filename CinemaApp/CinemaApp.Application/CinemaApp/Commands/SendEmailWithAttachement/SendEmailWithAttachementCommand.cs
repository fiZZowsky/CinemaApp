using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.SendEmailWithAttachement
{
    public class SendEmailWithAttachementCommand : TicketDto, IRequest
    {
        public string EmailTemplateText { get; set; }
        public byte[] Attachment { get; set; }

        public SendEmailWithAttachementCommand(string emailTemplateText, byte[] attachment)
        {
            EmailTemplateText = emailTemplateText;
            Attachment = attachment;
        }
    }
}
