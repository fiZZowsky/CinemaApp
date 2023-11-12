using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using CinemaApp.Domain.Interfaces;
using Moq;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovie.Tests
{
    [TestClass()]
    public class CreateMovieCommandValidatorTests
    {
        [TestMethod()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            // arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            movieRepositoryMock.Setup(repo => repo.IsMovieExist(It.IsAny<string>())).ReturnsAsync(false);
            var validator = new CreateMovieCommandValidator(movieRepositoryMock.Object);
            var command = new CreateMovieCommand()
            {
                Title = "Test",
                Genre = "Fantasy",
                Country = "Poland",
                Language = "polish",
                Duration = 120,
                Description = "Test",
                ReleaseDate = DateTime.Now
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod()]
        public void Validate_WithInvalidCommand_ShouldHaveValidationErrors()
        {
            // arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            movieRepositoryMock.Setup(repo => repo.IsMovieExist(It.IsAny<string>())).ReturnsAsync(true);
            var validator = new CreateMovieCommandValidator(movieRepositoryMock.Object);
            var command = new CreateMovieCommand()
            {
                Title = "",
                Genre = "",
                Country = "",
                Language = "",
                Duration = 0,
                Description = "",
                ReleaseDate = default(DateTime)
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.Title);
            result.ShouldHaveValidationErrorFor(c => c.Genre);
            result.ShouldHaveValidationErrorFor(c => c.Country);
            result.ShouldHaveValidationErrorFor(c => c.Language);
            result.ShouldHaveValidationErrorFor(c => c.Duration);
            result.ShouldHaveValidationErrorFor(c => c.Description);
            result.ShouldHaveValidationErrorFor(c => c.ReleaseDate);
        }

        [TestMethod()]
        public void Validate_WithTitleAlreadyExists_ShouldThrowValidationException()
        {
            // arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            movieRepositoryMock.Setup(repo => repo.IsMovieExist(It.IsAny<string>())).ReturnsAsync(true);

            var validator = new CreateMovieCommandValidator(movieRepositoryMock.Object);

            var command = new CreateMovieCommand()
            {
                Title = "ExistingMovieTitle",
                Genre = "Action",
                Country = "USA",
                Language = "English",
                Duration = 120,
                Description = "A great movie",
                ReleaseDate = DateTime.Now,
            };

            // act
            Action validationAction = () => validator.ValidateAndThrow(command);

            // assert
            validationAction.Should().Throw<FluentValidation.ValidationException>()
                .Where(ex => ex.Errors.Any(e => e.ErrorMessage == "A movie with the given title already exists."));
        }
    }
}