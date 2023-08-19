using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetSeat
{
    public class GetSeatQuery : IRequest<Domain.Entities.Seat>
    {
        public int HallNumber { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }

        public GetSeatQuery(int hallNumber, int rowNumber, int seatNumber) 
        {
            HallNumber = hallNumber;
            RowNumber = rowNumber;
            SeatNumber = seatNumber;
        }
    }
}
