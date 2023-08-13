using CinemaApp.Application.Dtos;

namespace CinemaApp.Application.Services
{
    public interface ITicketService
    {
        Task Create(TicketDto ticket, int movieShowId, int seatId);
        Task<IEnumerable<TicketDto>> GetAll();
    }
}