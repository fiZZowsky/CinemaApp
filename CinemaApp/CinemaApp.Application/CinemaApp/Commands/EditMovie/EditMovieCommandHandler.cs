using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.EditMovie
{
    public class EditMovieCommandHandler : IRequestHandler<EditMovieCommand>
    {
        private readonly IMovieRepository _movieRepository;

        public EditMovieCommandHandler(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<Unit> Handle(EditMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetMovieByEncodedTitle(request.EncodedTitle!);

            movie.Movie.Genre = request.Genre;
            movie.Movie.Country = request.Country;
            movie.Movie.AgeRating = request.AgeRating;
            movie.Movie.Language = request.Language;
            movie.Movie.Duration = request.Duration;
            movie.Movie.Description = request.Description;
            movie.Movie.ProductionYear = request.ProductionYear;
            movie.Movie.ReleaseDate = request.ReleaseDate;
            movie.StartTime = request.StartTime;
            movie.Hall.Number = request.HallNumber;

            
            await _movieRepository.Commit();

            return Unit.Value;
        }
    }
}
