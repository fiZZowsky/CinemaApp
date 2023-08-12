using CinemaApp.Application.Dtos;

namespace CinemaApp.Application.Services
{
    public interface IMovieShowService
    {
        Task<IEnumerable<MovieShowDto>> GetAll();
    }
}
