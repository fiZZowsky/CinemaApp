namespace CinemaApp.Application.Services
{
    public interface IMovieService
    {
        Task Create(Domain.Entities.Movie movie);
    }
}