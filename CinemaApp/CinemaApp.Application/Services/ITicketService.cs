using CinemaApp.Application.Dtos;

namespace CinemaApp.Application.Services
{
    public interface ITicketService
    {
        Task Create(TicketDto ticket);
    }
}