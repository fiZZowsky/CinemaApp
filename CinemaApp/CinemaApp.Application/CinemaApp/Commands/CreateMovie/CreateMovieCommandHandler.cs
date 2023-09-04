using AutoMapper;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovie
{
    public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateMovieCommandHandler(IMovieRepository movieRepository, IMapper mapper, IUserContext userContext)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if(currentUser == null || !currentUser.IsInRole("Admin"))
            {
                return Unit.Value;
            }

            var movie = _mapper.Map<Domain.Entities.MovieShow>(request);
            movie.Movie.EncodeTitle();

            await _movieRepository.Create(movie);

            return Unit.Value;
        }
    }
}
