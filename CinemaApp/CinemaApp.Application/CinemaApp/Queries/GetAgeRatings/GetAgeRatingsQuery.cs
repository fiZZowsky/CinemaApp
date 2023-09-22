using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAgeRatings
{
    public class GetAgeRatingsQuery : IRequest<IEnumerable<AgeRatingDto>>
    {
    }
}
