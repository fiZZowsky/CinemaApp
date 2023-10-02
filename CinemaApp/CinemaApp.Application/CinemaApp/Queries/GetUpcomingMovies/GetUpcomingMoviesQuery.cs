using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetUpcomingMovies
{
    public class GetUpcomingMoviesQuery : IRequest<IEnumerable<MovieDto>>
    {

    }
}
