using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetMovieRatings
{
    public class GetMovieRatingsQuery : IRequest<IEnumerable<RatingDto>>
    {
        public int MovieId { get; set; } = default!;
    }
}
