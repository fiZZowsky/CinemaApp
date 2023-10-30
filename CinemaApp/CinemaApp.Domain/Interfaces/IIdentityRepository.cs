using Microsoft.AspNetCore.Identity;

namespace CinemaApp.Domain.Interfaces
{
    public interface IIdentityRepository
    {
        Task SendEmailWithRecoveryPassword(string recipient, string emailTemplateText, string callback);
        Task SendConfirmationEmail(string recipient, string emailTemplateText, string callback);
    }
}
