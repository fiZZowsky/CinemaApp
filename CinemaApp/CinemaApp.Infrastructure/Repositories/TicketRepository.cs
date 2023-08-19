using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace CinemaApp.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly CinemaAppDbContext _dbContext;

        public TicketRepository(CinemaAppDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}
