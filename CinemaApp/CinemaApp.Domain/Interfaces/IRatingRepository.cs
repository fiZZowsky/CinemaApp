namespace CinemaApp.Domain.Interfaces
{
    public interface IRatingRepository
    {
        Task Create(Domain.Entities.Rating rating);
        Task<IEnumerable<Domain.Entities.Rating>> GetRatingsByMovieId(int movieId);
        Task DeleteMovieRating(int id, int movieId);
    }
}
