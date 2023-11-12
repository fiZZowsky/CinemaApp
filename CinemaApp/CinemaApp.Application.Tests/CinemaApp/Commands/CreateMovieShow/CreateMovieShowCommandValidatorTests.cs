using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaApp.Application.CinemaApp.Commands.CreateMovieShow;
using System;
using CinemaApp.Domain.Interfaces;
using Moq;
using CinemaApp.Domain.Entities;
using FluentValidation.TestHelper;
using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using FluentAssertions;
using FluentValidation;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovieShow.Tests
{
    [TestClass()]
    public class CreateMovieShowCommandValidatorTests
    {
        [TestMethod()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            // arrange
            var movieShowRepositoryMock = new Mock<IMovieShowRepository>();
            movieShowRepositoryMock.Setup(repo => repo.IsMoviePremiered(It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(true);
            movieShowRepositoryMock.Setup(repo => repo.IsHallBusy(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>())).ReturnsAsync(false);

            var movieRepositoryMock = new Mock<IMovieRepository>();
            movieRepositoryMock.Setup(repo => repo.GetMovieById(It.IsAny<int>())).ReturnsAsync(new Movie());

            var validator = new CreateMovieShowCommandValidator(movieShowRepositoryMock.Object, movieRepositoryMock.Object);

            var command = new CreateMovieShowCommand()
            {
                MovieId = 1,
                StartTime = DateTime.Now,
                HallId = 1
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
            var movieShowRepositoryMock = new Mock<IMovieShowRepository>();
            movieShowRepositoryMock.Setup(repo => repo.IsMoviePremiered(It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(true); ;
            movieShowRepositoryMock.Setup(repo => repo.IsHallBusy(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>())).ReturnsAsync(false);

            var movieRepositoryMock = new Mock<IMovieRepository>();
            movieRepositoryMock.Setup(repo => repo.GetMovieById(It.IsAny<int>())).ReturnsAsync(new Movie());
            var validator = new CreateMovieShowCommandValidator(movieShowRepositoryMock.Object, movieRepositoryMock.Object);
            var command = new CreateMovieShowCommand()
            {
                MovieId = 0,
                StartTime = default(DateTime),
                HallId = 0
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.MovieId);
            result.ShouldHaveValidationErrorFor(c => c.StartTime);
            result.ShouldHaveValidationErrorFor(c => c.HallId);
        }

        [TestMethod()]
        public void Validate_WithMovieNotPremieredAndBusyHall_ShouldThrowValidationException()
        {
            // arrange
            var movieShowRepositoryMock = new Mock<IMovieShowRepository>();
            movieShowRepositoryMock.Setup(repo => repo.IsMoviePremiered(It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(false); ;
            movieShowRepositoryMock.Setup(repo => repo.IsHallBusy(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>())).ReturnsAsync(true);

            var movieRepositoryMock = new Mock<IMovieRepository>();
            movieRepositoryMock.Setup(repo => repo.GetMovieById(It.IsAny<int>())).ReturnsAsync(new Movie());
            var validator = new CreateMovieShowCommandValidator(movieShowRepositoryMock.Object, movieRepositoryMock.Object);

            var command = new CreateMovieShowCommand()
            {
                MovieId = 1,
                StartTime = DateTime.Now,
                HallId = 1
            };

            // act
            Action validationAction = () => validator.ValidateAndThrow(command);

            // assert
            validationAction.Should().Throw<FluentValidation.ValidationException>()
                .Where(ex => ex.Errors.Any(e => e.ErrorMessage == "A film screening cannot take place before its premiere."));

            validationAction.Should().Throw<FluentValidation.ValidationException>()
                .Where(ex => ex.Errors.Any(e => e.ErrorMessage == "This hall is not available in this time."));
        }
    }
}
