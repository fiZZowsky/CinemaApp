using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetUnavailableSeats
{
    public class GetUnavailableSeatsQuery : IRequest<List<Domain.Entities.Seat>>
    {
        public int HallNumber { get; set; }
        public DateTime StartTime { get; set; }

        public GetUnavailableSeatsQuery(int hallNumber, DateTime startTime)
        {
            HallNumber = hallNumber;
            StartTime = startTime;
        }
    }
}
