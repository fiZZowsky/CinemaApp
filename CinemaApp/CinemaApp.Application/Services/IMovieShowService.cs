namespace CinemaApp.Application.Services
{
    public interface IMovieShowService
    {
        Task<Domain.Entities.MovieShow> GetByData(DateTime startTime, int hallNumber);
    }
}
