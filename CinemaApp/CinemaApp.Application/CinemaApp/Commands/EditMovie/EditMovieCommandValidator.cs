using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Domain.Interfaces;
using FluentValidation;

namespace CinemaApp.Application.CinemaApp.Commands.EditMovie
{
    public class EditMovieCommandValidator : AbstractValidator<CreateMovieCommand>
    {
        public EditMovieCommandValidator(IMovieRepository movieRepository)
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
                var startTime = context.InstanceToValidate.StartTime;
                var isHallBusy = movieRepository.IsHallBusy((int)hallNumber, startTime).Result;
                if (isHallBusy)
                {
                    context.AddFailure("This hall is not available in this time.");
                }
            });
        }
    }
}
