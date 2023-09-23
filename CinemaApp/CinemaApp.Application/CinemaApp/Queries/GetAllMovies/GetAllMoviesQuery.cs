using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAllMovies
{
    public class GetAllMoviesQuery : IRequest<IEnumerable<MovieDto>>
    {
    }
}
