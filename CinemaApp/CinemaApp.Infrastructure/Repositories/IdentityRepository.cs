using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly CinemaAppDbContext _dbContext;
        private readonly EmailSettings _emailSettings;

        public IdentityRepository(CinemaAppDbContext dbContext, IOptions<EmailSettings> emailSettings)
        {
            _dbContext = dbContext;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailWithRecoveryPassword(string recipient, string emailTemplateText, string callback)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port);
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);

                emailTemplateText = string.Format(emailTemplateText, recipient, DateTime.Today.Date.ToShortDateString());
                emailTemplateText = emailTemplateText.Replace("linkToPage", callback);

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = emailTemplateText
                };

                var message = new MimeMessage
                {
                    Body = bodyBuilder.ToMessageBody()
                };

                message.From.Add(new MailboxAddress("CinemaApp", _emailSettings.Username));
                message.To.Add(new MailboxAddress("User", recipient));
                message.Subject = "Recovery Password";
                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
        }
    }
}
