namespace CinemaApp.Domain.Interfaces
{
    public interface ITicketRepository
    {
        Task Create(Domain.Entities.Ticket ticket, int movieShowId, List<Domain.Entities.Seat> seats);
        Task<IEnumerable<Domain.Entities.Ticket>> GetAll();
        Task<IEnumerable<Domain.Entities.Ticket>> GetAllByUserId(string id);
        Task<byte[]> CreatePdf(string uid, string htmlContent);
        Task<Domain.Entities.Ticket> GetTicketByUser(string userId, DateTime purchaseDate, string movieTitle);
        Task SendEmailWithAttachement(string recipient, string emailTemplateText, byte[] attachement);
        Task<int> GetMaxSeatsNumber(int hallNumber);
        Task<Domain.Entities.Ticket> GetTicketByUid(string uid);
        Task<IEnumerable<Domain.Entities.Ticket>> GetTicketsBySearchString(string? uid);
        Task Commit();
    }
}
