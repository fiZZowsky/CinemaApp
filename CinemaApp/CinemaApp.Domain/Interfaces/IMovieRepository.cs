namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task Create(Domain.Entities.Movie movie);
        Task<IEnumerable<Domain.Entities.MovieShow>> GetAll();
        Task<Domain.Entities.MovieShow> GetMovieByEncodedTitle(string encodedTitle);
        Task Commit();
    }
}
