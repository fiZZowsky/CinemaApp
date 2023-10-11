using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Domain.Interfaces;
using FluentValidation;

namespace CinemaApp.Application.CinemaApp.Commands.EditMovie
{
    public class EditMovieCommandValidator : AbstractValidator<EditMovieCommand>
    {
        public EditMovieCommandValidator(IMovieRepository movieRepository)
        {
            RuleFor(m => m.Title).NotEmpty();
            RuleFor(m => m.Genre).NotEmpty();
            RuleFor(m => m.Country).NotEmpty();
            RuleFor(m => m.AgeRatingId).NotEmpty();
            RuleFor(m => m.Language).NotEmpty();
            RuleFor(m => m.Duration).NotEmpty();
            RuleFor(m => m.Description).NotEmpty();
            RuleFor(m => m.ReleaseDate).NotEmpty();
        }
    }
}
