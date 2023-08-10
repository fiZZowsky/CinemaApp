namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task Create(Domain.Entities.Movie movie);
    }
}
