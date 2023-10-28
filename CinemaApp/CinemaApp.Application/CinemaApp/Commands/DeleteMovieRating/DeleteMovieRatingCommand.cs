using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.DeleteMovieRating
{
    public class DeleteMovieRatingCommand : RatingDto, IRequest
    {
    }
}
