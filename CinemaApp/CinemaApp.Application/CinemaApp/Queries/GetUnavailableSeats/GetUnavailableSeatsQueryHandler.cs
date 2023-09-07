using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetUnavailableSeats
{
    public class GetUnavailableSeatsQueryHandler : IRequestHandler<GetUnavailableSeatsQuery, List<Domain.Entities.Seat>>
    {
        private readonly ISeatRepository _seatRepository;

        public GetUnavailableSeatsQueryHandler(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<List<Seat>> Handle(GetUnavailableSeatsQuery request, CancellationToken cancellationToken)
        {
            var unavailableSeats = await _seatRepository.GetUnavailableSeats(request.HallNumber, request.StartTime);
            return unavailableSeats;
        }
    }
}
