namespace CinemaApp.Domain.Interfaces
{
    public interface ISeatRepository
    {
        Task<Domain.Entities.Seat> GetByData(int hallNumber, int rowNumber, int seatNumber);
    }
}
