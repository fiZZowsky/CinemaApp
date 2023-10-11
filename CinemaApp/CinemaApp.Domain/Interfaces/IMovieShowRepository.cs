namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieShowRepository
    {
        Task<IEnumerable<Domain.Entities.MovieShow>> GetAll();
        Task Create(Domain.Entities.MovieShow show);
        Task<Domain.Entities.MovieShow> GetMovieShowByEncodedTitle(string encodedTitle);
        Task<Domain.Entities.MovieShow> GetByData(DateTime startTime, int hallNumber);
        Task<bool> IsHallBusy(int hallNumber, DateTime startTime, string movieTitle);
        Task<bool> IsMoviePremiered(int movieId, DateTime startTime);
        Task<IEnumerable<Domain.Entities.MovieShow>> GetRepertoire(List<int>? hallNumber, DateTime? startTime, string? searchString);
        Task Commit();
    }
}
