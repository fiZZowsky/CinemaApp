using CinemaApp.Domain.Interfaces;

namespace CinemaApp.Application.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }
        public async Task<Domain.Entities.Seat> GetByData(int hallNumber, int rowNumber, int seatNumber)
        {
            var seat = await _seatRepository.GetByData(hallNumber, rowNumber, seatNumber);
            return seat;
        }
    }
}
