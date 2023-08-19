using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetSeat
{
    public class GetSeatQueryHandler : IRequestHandler<GetSeatQuery, Domain.Entities.Seat>
    {
        private readonly ISeatRepository _seatRepository;

        public GetSeatQueryHandler(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<Domain.Entities.Seat> Handle(GetSeatQuery request, CancellationToken cancellationToken)
        {
            var seat = await _seatRepository.GetByData(request.HallNumber, request.RowNumber, request.SeatNumber);
            return seat;
        }
    }
}
