using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetSeat
{
    public class GetSeatQuery : IRequest<List<Domain.Entities.Seat>>
    {
        public int HallNumber { get; set; }
        public List<int> RowNumber { get; set; }
        public List<int> SeatNumber { get; set; }

        public GetSeatQuery(int hallNumber, List<int> rowNumber, List<int> seatNumber)
        {
            HallNumber = hallNumber;
            RowNumber = rowNumber;
            SeatNumber = seatNumber;
        }
    }
}
