namespace CinemaApp.Application.Services
{
    public interface ITicketService
    {
        Task Create(Domain.Entities.Ticket ticket);
    }
}