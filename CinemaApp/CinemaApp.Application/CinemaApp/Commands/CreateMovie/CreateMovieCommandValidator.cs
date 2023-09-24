using CinemaApp.Domain.Interfaces;
using FluentValidation;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovie
{
    public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
    {
        public CreateMovieCommandValidator(IMovieRepository movieRepository)
        {
            RuleFor(m => m.Title)
                .NotEmpty()
                .Custom((title, content) =>
                {
                    var isMovieExist = movieRepository.IsMovieExist(title).Result;
                    if(isMovieExist)
                    {
                        content.AddFailure("A movie with the given title already exists.");
                    }
                });
            RuleFor(m => m.Genre).NotEmpty();
            RuleFor(m => m.Country).NotEmpty();
            RuleFor(m => m.Language).NotEmpty();
            RuleFor(m => m.Duration).NotEmpty();
            RuleFor(m => m.Description).NotEmpty();
            RuleFor(m => m.ReleaseDate).NotEmpty();
        }
    }
}
