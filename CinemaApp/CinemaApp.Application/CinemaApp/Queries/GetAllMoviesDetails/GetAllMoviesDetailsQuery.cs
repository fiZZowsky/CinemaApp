using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAllMoviesDetails
{
    public class GetAllMoviesDetailsQuery : IRequest<IEnumerable<MovieDetailsDto>>
    {

    }
}
