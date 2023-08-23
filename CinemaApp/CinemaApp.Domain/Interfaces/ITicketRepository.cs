namespace CinemaApp.Domain.Interfaces
{
    public interface ITicketRepository
    {
        Task Create(Domain.Entities.Ticket ticket, int movieShowId, int seatId);
        Task<IEnumerable<Domain.Entities.Ticket>> GetAll();
        Task<byte[]> CreatePdf(int id, string htmlContent);
        Task<Domain.Entities.Ticket> GetTicketByUser(string userId, DateTime purchaseDate, string movieTitle);
        Task SendEmailWithAttachement(string recipient, string emailTemplateText, byte[] attachement);
    }
}
