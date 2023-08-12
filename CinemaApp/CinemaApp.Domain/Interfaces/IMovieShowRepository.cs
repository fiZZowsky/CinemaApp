namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieShowRepository
    {
        Task<IEnumerable<Domain.Entities.MovieShow>> GetAll();
    }
}
