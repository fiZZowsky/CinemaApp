using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.EditMovieShow
{
    public class EditMovieShowCommandHandler : IRequestHandler<EditMovieShowCommand>
    {
        private readonly IMovieShowRepository _movieShowRepository;
        private readonly IUserContext _userContext;

        public EditMovieShowCommandHandler(IMovieShowRepository movieShowRepository, IUserContext userContext)
        {
            _movieShowRepository = movieShowRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(EditMovieShowCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.IsInRole("Admin"))
            {
                return Unit.Value;
            }

            var movie = await _movieShowRepository.GetMovieShowByEncodedTitle(request.EncodedTitle!);

            movie.Movie.Genre = request.Genre;
            movie.Movie.Country = request.Country;
            movie.Movie.AgeRatingId = request.AgeRatingId;
            movie.Movie.Language = request.Language;
            movie.Movie.Duration = request.Duration;
            movie.Movie.Description = request.Description;
            movie.Movie.ReleaseDate = request.ReleaseDate;
            movie.StartTime = request.StartTime;
            movie.Hall.Number = (int)request.HallNumber;

            await _movieShowRepository.Commit();

            return Unit.Value;
        }
    }
}
