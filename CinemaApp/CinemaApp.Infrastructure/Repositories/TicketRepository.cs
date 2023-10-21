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

        public async Task Create(Domain.Entities.Ticket ticket, int movieShowId, List<Domain.Entities.Seat> seats)
        {
            ticket.Guid = GenerateNewGuid();
            ticket.MovieShowId = movieShowId;
            ticket.Seats = seats;

            ticket.QRCode = await CreateQRCode(ticket);

            _dbContext.Add(ticket);
            await _dbContext.SaveChangesAsync();
        }

        private async Task<byte[]> CreateQRCode(Domain.Entities.Ticket ticket)
        {
            string data = $"https://10.0.2.2:7190/Ticket/TicketCheck/{ticket.Guid}";

            using (MemoryStream ms = new MemoryStream())
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeImageBytes = qrCode.GetGraphic(2);

                return await Task.FromResult(qrCodeImageBytes);
            }
        }

        private static Guid GenerateNewGuid()
        {
            return Guid.NewGuid();
        }

        public async Task<IEnumerable<Ticket>> GetAll()
            => await _dbContext.Tickets
                .Include(ticket => ticket.MovieShow)
                    .ThenInclude(show => show.Movie)
                .Include(ticket => ticket.Seats)
                    .ThenInclude(seat => seat.Hall)
                .OrderBy(ticket => ticket.PurchaseDate)
                .ToListAsync();

        public async Task<Domain.Entities.Ticket> GetTicketByGuid(Guid guid)
            => await _dbContext.Tickets
            .Include(ticket => ticket.MovieShow)
                .ThenInclude(show => show.Movie)
            .Include(ticket => ticket.Seats)
                .ThenInclude(seat => seat.Hall)
            .FirstAsync(ticket => ticket.Guid == guid);

        public async Task<byte[]> CreatePdf(Guid guid, string htmlContent)
        {
            var ticket = await GetTicketByGuid(guid);

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Ticket " + guid
            };

            string base64Image = Convert.ToBase64String(ticket.QRCode);
            string QRCode = $"data:image/png;base64,{base64Image}";

            var seatNumbers = string.Join(", ", ticket.Seats.Select(seat => seat.Number.ToString()));
            var rowNumbers = string.Join(", ", ticket.Seats.Select(seat => seat.RowNumber.ToString()).Distinct());

            htmlContent = htmlContent
                .Replace("@Model.Guid", guid.ToString())
                .Replace("@Model.Title", ticket.MovieShow.Movie.Title)
                .Replace("@Model.Language", ticket.MovieShow.Movie.Language)
                .Replace("@Model.Duration", ticket.MovieShow.Movie.Duration.ToString())
                .Replace("@Model.StartTime", ticket.MovieShow.StartTime.ToString("dd-MM-yyyy HH:mm"))
                .Replace("@Model.HallNumber", ticket.MovieShow.Hall.Number.ToString())
                .Replace("@Model.RowNumber", rowNumbers)
                .Replace("@Model.SeatNumber", seatNumbers)
                .Replace("@Model.QRCode", QRCode);

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlContent,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = @"wwwroot\css\Ticket\PdfWithTicketStyleSheet.css" }
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
            .Include(ticket => ticket.MovieShow)
                .ThenInclude(show => show.Movie)
            .Include(ticket => ticket.Seats)
                .ThenInclude(seat => seat.Hall)
            .FirstAsync(ticket => ticket.PurchasedById == userId
                && ticket.PurchaseDate == purchaseDate
                && ticket.MovieShow.Movie.Title == movieTitle);

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

        public async Task<int> GetMaxSeatsNumber(int hallNumber)
        {
            var hall = await _dbContext.Halls
                .SingleOrDefaultAsync(h => h.Number == hallNumber);

            return hall.NumberOfRows * hall.PlacesInARow;
        }

        public async Task Commit()
            => await _dbContext.SaveChangesAsync();

        public async Task<IEnumerable<Ticket>> GetAllByUserId(string id)
            => await _dbContext.Tickets
            .Include(ticket => ticket.MovieShow)
                .ThenInclude(show => show.Movie)
            .Include(ticket => ticket.Seats)
                .ThenInclude(seat => seat.Hall)
            .Where(ticket => ticket.PurchasedById == id)
            .OrderBy(ticket => ticket.PurchaseDate)
            .ToListAsync();
    }
}
