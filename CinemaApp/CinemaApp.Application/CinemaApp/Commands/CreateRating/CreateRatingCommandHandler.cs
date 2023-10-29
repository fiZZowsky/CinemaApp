using AutoMapper;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateRating
{
    public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public CreateRatingCommandHandler(IRatingRepository ratingRepository, IMovieRepository movieRepository, IMapper mapper, IUserContext userContext)
        {
            _ratingRepository = ratingRepository;
            _movieRepository = movieRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
            {
                return Unit.Value;
            }

            var movie = await _movieRepository.GetMovieById(request.MovieId);

            var rating = _mapper.Map<Domain.Entities.Rating>(request);

            rating.CreatedByUserId = currentUser.Id;
            rating.CreatedBy = currentUser.Email;
            await _ratingRepository.Create(rating);

            movie.CountRate();

            await _movieRepository.Commit();

            return Unit.Value;
        }
    }
}
