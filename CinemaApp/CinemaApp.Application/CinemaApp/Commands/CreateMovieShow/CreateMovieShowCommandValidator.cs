using CinemaApp.Domain.Interfaces;
using FluentValidation;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovieShow
{
    public class CreateMovieShowCommandValidator : AbstractValidator<CreateMovieShowCommand>
    {
        public CreateMovieShowCommandValidator(IMovieShowRepository movieShowRepository, IMovieRepository movieRepository)
        {
            RuleFor(ms => ms.MovieId).NotEmpty();
            RuleFor(ms => ms.StartTime)
                .NotEmpty()
                .Custom((startTime, context) =>
                {
                    var movieId = context.InstanceToValidate.MovieId;
                    var isMoviePremiered = movieShowRepository.IsMoviePremiered(movieId, startTime).Result;
                    if (!isMoviePremiered)
                    {
                        context.AddFailure("A film screening cannot take place before its premiere.");
                    }
                });
            RuleFor(ms => ms.HallId)
                .NotEmpty()
                .Custom((hallId, context) =>
                {
                    var startTime = context.InstanceToValidate.StartTime;
                    var movie = movieRepository.GetMovieById(context.InstanceToValidate.MovieId);

                    var isHallBusy = movieShowRepository.IsHallBusy(hallId, startTime, movie.Result.Title).Result;
                    if (isHallBusy)
                    {
                        context.AddFailure("This hall is not available in this time.");
                    }
                });
        }
    }
}
