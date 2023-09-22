namespace CinemaApp.Domain.Interfaces
{
    public interface IAgeRatingRepository
    {
        Task<IEnumerable<Domain.Entities.AgeRating>> GetAgeRatings();
        Task<Domain.Entities.AgeRating> GetAgeRatingById(int ageRatingId);
    }
}
