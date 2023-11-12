using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Domain.Interfaces;
using FluentValidation;

namespace CinemaApp.Application.CinemaApp.Commands.EditMovieShow
{
    public class EditMovieShowCommandValidator : AbstractValidator<EditMovieShowCommand>
    {
        public EditMovieShowCommandValidator(IMovieShowRepository movieShowRepository)
        {
            RuleFor(m => m.Genre).NotEmpty();
            RuleFor(m => m.Country).NotEmpty();
            RuleFor(m => m.AgeRatingId).NotEmpty();
            RuleFor(m => m.Language).NotEmpty();
            RuleFor(m => m.Duration).NotEmpty();
            RuleFor(m => m.Description).NotEmpty();
            RuleFor(m => m.ReleaseDate).NotEmpty();
            RuleFor(m => m.StartTime).NotEmpty();
            RuleFor(m => m.HallNumber)
                .NotEmpty()
                .Custom((hallNumber, context) =>
            {
                if (hallNumber.HasValue)
                {
                    var startTime = context.InstanceToValidate.StartTime;
                    var movieTitle = context.InstanceToValidate.Title;

                    var isHallBusy = movieShowRepository.IsHallBusy(hallNumber.Value, startTime, movieTitle).Result;
                    if (isHallBusy)
                    {
                        context.AddFailure("This hall is not available in this time.");
                    }
                }
            });
        }
    }
}
