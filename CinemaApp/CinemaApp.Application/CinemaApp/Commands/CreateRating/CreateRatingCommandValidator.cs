using FluentValidation;

namespace CinemaApp.Application.CinemaApp.Commands.CreateRating
{
    public class CreateRatingCommandValidator : AbstractValidator<CreateRatingCommand>
    {
        public CreateRatingCommandValidator() 
        {
            RuleFor(r => r.RateValue)
                .NotEmpty()
                .NotNull()
                .InclusiveBetween(0, 5)
                .WithMessage("Rate value must be between 0 and 5.");
            RuleFor(r => r.Comment).NotEmpty().NotNull();
            RuleFor(r => r.MovieId).NotEmpty().NotNull();
        }
    }
}
