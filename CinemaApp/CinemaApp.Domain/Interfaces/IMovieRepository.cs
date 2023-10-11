namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task Create(Domain.Entities.Movie movie);
        Task<IEnumerable<Domain.Entities.Movie>> GetAll();
        Task<bool> IsMovieExist(string title);
        Task<IEnumerable<Domain.Entities.Movie>> GetUpcomingMovies();
        Task<Domain.Entities.Movie> GetMovieByEncodedTitle(string encodedTitle);
        Task<Domain.Entities.Movie> GetMovieById(int id);
        Task Commit();
    }
}
