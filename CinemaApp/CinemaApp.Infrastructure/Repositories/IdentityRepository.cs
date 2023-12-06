using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CinemaApp.Infrastructure.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly CinemaAppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EmailSettings _emailSettings;

        public IdentityRepository(CinemaAppDbContext dbContext, IOptions<EmailSettings> emailSettings, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendConfirmationEmail(string recipient, string emailTemplateText, string callback)
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
                message.Subject = "Email Confirmation";
                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
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

        public async Task<IEnumerable<CinemaApp.Domain.Entities.User>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<CinemaApp.Domain.Entities.User>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userList.Add(new CinemaApp.Domain.Entities.User
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Role = roles.FirstOrDefault()
                });
            }

            return userList;
        }

        public async Task ChangeUserRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            await _userManager.AddToRoleAsync(user, roleName);
        }
    }
}
