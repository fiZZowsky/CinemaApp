using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.DeleteMovieRating
{
    public class DeleteMovieRatingCommandHandler : IRequestHandler<DeleteMovieRatingCommand>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMovieRepository _movieRepository;

        public DeleteMovieRatingCommandHandler(IRatingRepository ratingRepository, IMovieRepository movieRepository)
        {
            _ratingRepository = ratingRepository;
            _movieRepository = movieRepository;
        }

        public async Task<Unit> Handle(DeleteMovieRatingCommand request, CancellationToken cancellationToken)
        {
            await _ratingRepository.DeleteMovieRating(request.Id, request.MovieId);
            var movie = await _movieRepository.GetMovieById(request.MovieId);

            await _movieRepository.RemoveRatingFromList(request.Id, request.MovieId);
            movie.CountRate();
            await _movieRepository.Commit();

            return Unit.Value;
        }
    }
}
