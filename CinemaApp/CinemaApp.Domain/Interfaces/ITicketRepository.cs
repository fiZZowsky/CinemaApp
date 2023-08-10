namespace CinemaApp.Domain.Interfaces
{
    public interface ITicketRepository
    {
        Task Create(Domain.Entities.Ticket ticket);
    }
}
