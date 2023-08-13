namespace CinemaApp.Application.Services
{
    public interface ISeatService
    {
        Task<Domain.Entities.Seat> GetByData(int hallNumber, int rowNumber, int seatNumber);
    }
}
