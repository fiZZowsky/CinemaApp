using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaApp.Application.CinemaApp.Commands.EditMovieShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.Application.CinemaApp.Commands.EditMovie;
using CinemaApp.Domain.Interfaces;
using Moq;
using CinemaApp.Application.CinemaApp.Commands.CreateMovieShow;
using CinemaApp.Domain.Entities;
using FluentValidation.TestHelper;
using FluentAssertions;
using FluentValidation;

namespace CinemaApp.Application.CinemaApp.Commands.EditMovieShow.Tests
{
    [TestClass()]
    public class EditMovieShowCommandValidatorTests
    {
        [TestMethod()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            // arrange
            var movieShowRepositoryMock = new Mock<IMovieShowRepository>();
            movieShowRepositoryMock.Setup(repo => repo.IsMoviePremiered(It.IsAny<int>(), It.IsAny<DateTime>())).ReturnsAsync(true);
            movieShowRepositoryMock.Setup(repo => repo.IsHallBusy(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>())).ReturnsAsync(false);

            var validator = new EditMovieShowCommandValidator(movieShowRepositoryMock.Object);
            var command = new EditMovieShowCommand()
            {
                Genre = "Sci-Fi",
                Country = "United States",
                AgeRatingId = 1,
                Language = "english",
                Duration = 180,
                Description = "Test",
                ReleaseDate = DateTime.Now,
                StartTime = DateTime.Now,
                HallNumber = 1
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

            var validator = new EditMovieShowCommandValidator(movieShowRepositoryMock.Object);
            var command = new EditMovieShowCommand()
            {
                Genre = "",
                Country = "",
                AgeRatingId = 0,
                Language = "",
                Duration = 0,
                Description = "",
                ReleaseDate = default(DateTime),
                StartTime = default(DateTime),
                HallNumber = null
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.Genre);
            result.ShouldHaveValidationErrorFor(c => c.Country);
            result.ShouldHaveValidationErrorFor(c => c.AgeRatingId);
            result.ShouldHaveValidationErrorFor(c => c.Language);
            result.ShouldHaveValidationErrorFor(c => c.Duration);
            result.ShouldHaveValidationErrorFor(c => c.Description);
            result.ShouldHaveValidationErrorFor(c => c.ReleaseDate);
            result.ShouldHaveValidationErrorFor(c => c.StartTime);
            result.ShouldHaveValidationErrorFor(c => c.HallNumber);
        }

        [TestMethod()]
        public async Task Validate_WhenHallIsBusy_ShouldThrowValidationException()
        {
            // arrange
            var movieShowRepositoryMock = new Mock<IMovieShowRepository>();
            movieShowRepositoryMock.Setup(repo => repo.IsHallBusy(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<string>())).ReturnsAsync(true);

            var validator = new EditMovieShowCommandValidator(movieShowRepositoryMock.Object);
            var command = new EditMovieShowCommand()
            {
                Genre = "Sci-Fi",
                Country = "United States",
                AgeRatingId = 1,
                Language = "english",
                Duration = 180,
                Description = "Test",
                ReleaseDate = DateTime.Now,
                StartTime = DateTime.Now,
                HallNumber = 1
            };

            // act
            Func<Task> validationAction = async () => await validator.ValidateAndThrowAsync(command);

            // assert
            await validationAction.Should().ThrowAsync<FluentValidation.ValidationException>()
                .WithMessage("*HallNumber*This hall is not available in this time*");
        }
    }
}