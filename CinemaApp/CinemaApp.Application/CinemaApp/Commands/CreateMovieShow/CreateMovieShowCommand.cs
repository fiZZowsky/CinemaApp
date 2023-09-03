using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovieShow
{
    public class CreateMovieShowCommand : Domain.Entities.MovieShow, IRequest
    {
    }
}
