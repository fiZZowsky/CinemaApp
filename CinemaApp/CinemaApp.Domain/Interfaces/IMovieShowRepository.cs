namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieShowRepository
    {
        Task<Domain.Entities.MovieShow> GetByData(DateTime startTime, int hallNumber);
    }
}
