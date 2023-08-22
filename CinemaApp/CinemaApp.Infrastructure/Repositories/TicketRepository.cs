using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace CinemaApp.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly CinemaAppDbContext _dbContext;
        private readonly IConverter _converter;

        public TicketRepository(CinemaAppDbContext dbContext, IConverter converter)
        {
            _dbContext = dbContext;
            _converter = converter;
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
    }
}
