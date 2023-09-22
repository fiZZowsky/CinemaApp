using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAgeRatingById
{
    public class GetAgeRatingByIdQuery : IRequest<AgeRatingDto>
    {
        public int AgeRatingId { get; set; }

        public GetAgeRatingByIdQuery(int ageRatingId)
        {
            AgeRatingId = ageRatingId;
        }
    }
}
