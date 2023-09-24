namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task Create(Domain.Entities.Movie movie);
        Task<IEnumerable<Domain.Entities.Movie>> GetAll();
        Task<Domain.Entities.MovieShow> GetMovieByEncodedTitle(string encodedTitle);
        Task<bool> IsHallBusy(int hallNumber, DateTime startTime);
        Task<bool> IsMovieExist(string title);
        Task Commit();
    }
}
