using FluentValidation;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovie
{
    public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
    {
        public CreateMovieCommandValidator()
        {
            RuleFor(m => m.Title).NotEmpty();
            RuleFor(m => m.Genre).NotEmpty();
            RuleFor(m => m.Country).NotEmpty();
            RuleFor(m => m.Language).NotEmpty();
            RuleFor(m => m.Duration).NotEmpty();
            RuleFor(m => m.Description).NotEmpty();
            RuleFor(m => m.ReleaseDate).NotEmpty();
        }
    }
}
