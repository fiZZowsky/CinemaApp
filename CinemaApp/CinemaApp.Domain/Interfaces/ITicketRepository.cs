namespace CinemaApp.Domain.Interfaces
{
    public interface ITicketRepository
    {
        Task Create(Domain.Entities.Ticket ticket, int movieShowId, int seatId);
        Task<IEnumerable<Domain.Entities.Ticket>> GetAll();
        Task<byte[]> CreatePdf(int id, string htmlContent);
    }
}
