using CinemaApp.Application.CinemaApp;

namespace CinemaApp.Application.Services
{
    public interface ITicketService
    {
        Task Create(TicketDto ticket, int movieShowId, int seatId);
        Task<IEnumerable<TicketDto>> GetAll();
    }
}