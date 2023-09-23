using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAllMoviesShows
{
    public class GetAllMoviesShowsQuery : IRequest<IEnumerable<MovieDto>>
    {

    }
}
