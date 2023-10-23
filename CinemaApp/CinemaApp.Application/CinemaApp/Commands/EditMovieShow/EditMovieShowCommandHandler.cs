using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.EditMovieShow
{
    public class EditMovieShowCommandHandler : IRequestHandler<EditMovieShowCommand>
    {
        private readonly IMovieShowRepository _movieShowRepository;
        private readonly IHallRepository _hallRepository;
        private readonly IUserContext _userContext;

        public EditMovieShowCommandHandler(IMovieShowRepository movieShowRepository, IHallRepository hallRepository, IUserContext userContext)
        {
            _movieShowRepository = movieShowRepository;
            _hallRepository = hallRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(EditMovieShowCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.IsInRole("Admin"))
            {
                return Unit.Value;
            }

            var show = await _movieShowRepository.GetMovieShowByEncodedTitle(request.EncodedTitle!, request.StartTime);
            var hall = await _hallRepository.GetHallByNumber((int)request.HallNumber!);

            show.Movie.Genre = request.Genre;
            show.Movie.Country = request.Country;
            show.Movie.AgeRatingId = request.AgeRatingId;
            show.Movie.Language = request.Language;
            show.Movie.Duration = request.Duration;
            show.Movie.Description = request.Description;
            show.Movie.ReleaseDate = request.ReleaseDate;
            show.StartTime = request.StartTime;
            show.HallId = hall.Id;

            await _movieShowRepository.Commit();

            return Unit.Value;
        }
    }
}
