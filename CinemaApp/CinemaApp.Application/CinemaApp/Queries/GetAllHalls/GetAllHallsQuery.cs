using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAllHalls.GetAllHalls
{
    public class GetAllHallsQuery : IRequest<IEnumerable<HallDto>>
    {
    }
}
