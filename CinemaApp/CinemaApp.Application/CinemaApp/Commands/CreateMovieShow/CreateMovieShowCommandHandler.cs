using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovieShow
{
    public class CreateMovieShowCommandHandler : IRequestHandler<CreateMovieShowCommand>
    {
        private readonly IMovieShowRepository _movieShowRepository;
        private readonly IUserContext _userContext;

        public CreateMovieShowCommandHandler(IMovieShowRepository movieShowRepository, IUserContext userContext)
        {
            _movieShowRepository = movieShowRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(CreateMovieShowCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null || !currentUser.IsInRole("Admin"))
            {
                return Unit.Value;
            }

            await _movieShowRepository.Create(request);

            return Unit.Value;
        }
    }
}
