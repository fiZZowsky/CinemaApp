using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using DinkToPdf;
using DinkToPdf.Contracts;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using QRCoder;

namespace CinemaApp.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly CinemaAppDbContext _dbContext;
        private readonly IConverter _converter;
        private readonly EmailSettings _emailSettings;

        public TicketRepository(CinemaAppDbContext dbContext, IConverter converter, IOptions<EmailSettings> emailSettings)
        {
            _dbContext = dbContext;
            _converter = converter;
            _emailSettings = emailSettings.Value;
        }
        public async Task Create(Domain.Entities.Ticket ticket, int movieShowId, int seatId)
        {
            ticket.MovieShowId = movieShowId;
            ticket.SeatId = seatId;

            ticket.QRCode = await CreateQRCode(ticket);

            _dbContext.Add(ticket);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<byte[]> CreateQRCode(Domain.Entities.Ticket ticket)
        {
            var seat = await _dbContext.Seats.Include(h => h.Hall)
                .FirstOrDefaultAsync(s => s.Id == ticket.SeatId);

            var movieShow = await _dbContext.MovieShows.Include(m => m.Movie)
                .FirstOrDefaultAsync(ms => ms.Id == ticket.MovieShowId);

            string data = $"{movieShow.Movie.Title}, {seat.Hall.Number}, {seat.RowNumber}, {seat.Number}, {ticket.Type}, {ticket.IsScanned}, {ticket.PurchaseDate}";

            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeImageBytes = qrCode.GetGraphic(2);

                return await Task.FromResult(qrCodeImageBytes);
            }
        }

        public async Task<IEnumerable<Ticket>> GetAll()
            => await _dbContext.Tickets
                .Include(s => s.MovieShow)
                    .ThenInclude(m => m.Movie)
                .Include(ticket => ticket.Seat)
                    .ThenInclude(h => h.Hall)
                .ToListAsync();

        private async Task<Ticket> GetTicketById(int id)
            => await _dbContext.Tickets
            .Include(s => s.MovieShow)
                .ThenInclude(m => m.Movie)
            .Include(ticket => ticket.Seat)
                .ThenInclude(h => h.Hall)
            .FirstAsync(t => t.Id == id);

        public async Task<byte[]> CreatePdf(int id, string htmlContent)
        {
            var ticket = await GetTicketById(id);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Ticket"
            };

            string base64Image = Convert.ToBase64String(ticket.QRCode);
            string QRCode = $"data:image/png;base64,{base64Image}";

            htmlContent = htmlContent
                .Replace("@Model.Title", ticket.MovieShow.Movie.Title)
                .Replace("@Model.Language", ticket.MovieShow.Movie.Language)
                .Replace("@Model.Duration", ticket.MovieShow.Movie.Duration.ToString())
                .Replace("@Model.StartTime", ticket.MovieShow.StartTime.ToString())
                .Replace("@Model.HallNumber", ticket.MovieShow.Hall.Number.ToString())
                .Replace("@Model.RowNumber", ticket.Seat.RowNumber.ToString())
                .Replace("@Model.SeatNumber", ticket.Seat.Number.ToString())
                .Replace("@Model.QRCode", QRCode);

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return _converter.Convert(pdf);
        }

        public async Task<Ticket> GetTicketByUser(string userId, DateTime purchaseDate, string movieTitle)
            => await _dbContext.Tickets
            .Include(s => s.MovieShow)
                .ThenInclude(m => m.Movie)
            .Include(ticket => ticket.Seat)
                .ThenInclude(h => h.Hall)
            .FirstAsync(t => t.PurchasedById == userId
                && t.PurchaseDate == purchaseDate
                && t.MovieShow.Movie.Title == movieTitle);

        public async Task SendEmailWithAttachement(string recipient, string emailTemplateText, byte[] attachement)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port);
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);

                emailTemplateText = string.Format(emailTemplateText, recipient, DateTime.Today.Date.ToShortDateString());

                var attachment = new MimePart("application", "pdf")
                {
                    Content = new MimeContent(new MemoryStream(attachement)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "ticket.pdf"
                };

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = emailTemplateText
                };

                bodyBuilder.Attachments.Add(attachment);

                var message = new MimeMessage
                {
                    Body = bodyBuilder.ToMessageBody()
                };

                message.From.Add(new MailboxAddress("CinemaApp", _emailSettings.Username));
                message.To.Add(new MailboxAddress("User", recipient));
                message.Subject = "Ticket";
                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
        }
    }
}
