namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task Create(Domain.Entities.MovieShow movie);
        Task<IEnumerable<Domain.Entities.MovieShow>> GetAll();
    }
}
