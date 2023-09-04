namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieShowRepository
    {
        Task Create(Domain.Entities.MovieShow show);
        Task<Domain.Entities.MovieShow> GetByData(DateTime startTime, int hallNumber);
        Task<bool> IsHallBusy(int hallId, DateTime startTime);
    }
}
