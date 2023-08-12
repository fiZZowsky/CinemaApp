using CinemaApp.Application.Dtos;

namespace CinemaApp.Application.Services
{
    public interface IMovieDetailsService
    {
        Task Create(MovieDetailsDto movieDto);
    }
}