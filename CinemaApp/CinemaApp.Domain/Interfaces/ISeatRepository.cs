namespace CinemaApp.Domain.Interfaces
{
    public interface ISeatRepository
    {
        Task<List<Domain.Entities.Seat>> GetByData(int hallNumber, List<int> rowNumber, List<int> seatNumber);
        Task<List<Domain.Entities.Seat>> GetUnavailableSeats(int hallNumber, DateTime startTime);
    }
}
