namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task Create(Domain.Entities.MovieShow movie);
        Task<IEnumerable<Domain.Entities.MovieShow>> GetAll();
        Task<Domain.Entities.MovieShow> GetMovieByEncodedTitle(string encodedTitle);
        Task Commit();
    }
}
