namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieDetailsRepository
    {
        Task Create(Domain.Entities.MovieShow movie);
        Task<IEnumerable<Domain.Entities.MovieShow>> GetAll();
    }
}
