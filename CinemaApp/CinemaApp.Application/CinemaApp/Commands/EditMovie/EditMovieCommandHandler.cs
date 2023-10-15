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

            movie.Genre = request.Genre;
            movie.Country = request.Country;
            movie.AgeRatingId = request.AgeRatingId;
            movie.Language = request.Language;
            movie.Duration = request.Duration;
            movie.Description = request.Description;
            movie.ReleaseDate = request.ReleaseDate;
            movie.PriceList.NormalTicketPrice = request.NormalTicketPrice;
            movie.PriceList.ReducedTicketPrice = request.ReducedTicketPrice;

            await _movieRepository.Commit();

            return Unit.Value;
        }
    }
}