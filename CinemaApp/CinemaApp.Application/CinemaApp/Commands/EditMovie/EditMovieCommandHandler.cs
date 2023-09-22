using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.EditMovie
{
    public class EditMovieCommandHandler : IRequestHandler<EditMovieCommand>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUserContext _userContext;

        public EditMovieCommandHandler(IMovieRepository movieRepository, IUserContext userContext)
        {
            _movieRepository = movieRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(EditMovieCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.IsInRole("Admin"))
            {
                return Unit.Value;
            }

            var movie = await _movieRepository.GetMovieByEncodedTitle(request.EncodedTitle!);

            movie.Movie.Genre = request.Genre;
            movie.Movie.Country = request.Country;
            movie.Movie.AgeRatingId = request.AgeRatingId;
            movie.Movie.Language = request.Language;
            movie.Movie.Duration = request.Duration;
            movie.Movie.Description = request.Description;
            movie.Movie.ReleaseDate = request.ReleaseDate;
            movie.StartTime = (DateTime)request.StartTime;
            movie.Hall.Number = (int)request.HallNumber;

            await _movieRepository.Commit();

            return Unit.Value;
        }
    }
}
