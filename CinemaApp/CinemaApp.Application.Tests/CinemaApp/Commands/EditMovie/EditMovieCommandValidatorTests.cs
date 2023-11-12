using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaApp.Application.CinemaApp.Commands.EditMovie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Domain.Interfaces;
using Moq;
using FluentValidation.TestHelper;

namespace CinemaApp.Application.CinemaApp.Commands.EditMovie.Tests
{
    [TestClass()]
    public class EditMovieCommandValidatorTests
    {
        [TestMethod()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            // arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            movieRepositoryMock.Setup(repo => repo.IsMovieExist(It.IsAny<string>())).ReturnsAsync(true);
            var validator = new EditMovieCommandValidator(movieRepositoryMock.Object);
            var command = new EditMovieCommand()
            {
                Title = "Test",
                Genre = "Fantasy",
                Country = "Poland",
                AgeRatingId = 1,
                Language = "polish",
                Duration = 120,
                Description = "Test",
                ReleaseDate = DateTime.Now,
                NormalTicketPrice = 2000,
                ReducedTicketPrice = 1500
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
            var validator = new EditMovieCommandValidator(movieRepositoryMock.Object);
            var command = new EditMovieCommand()
            {
                Title = "",
                Genre = "",
                Country = "",
                AgeRatingId = 0,
                Language = "",
                Duration = 0,
                Description = "",
                ReleaseDate = default(DateTime),
                NormalTicketPrice = 0,
                ReducedTicketPrice = 0
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.Title);
            result.ShouldHaveValidationErrorFor(c => c.Genre);
            result.ShouldHaveValidationErrorFor(c => c.Country);
            result.ShouldHaveValidationErrorFor(c => c.AgeRatingId);
            result.ShouldHaveValidationErrorFor(c => c.Language);
            result.ShouldHaveValidationErrorFor(c => c.Duration);
            result.ShouldHaveValidationErrorFor(c => c.Description);
            result.ShouldHaveValidationErrorFor(c => c.ReleaseDate);
            result.ShouldHaveValidationErrorFor(c => c.NormalTicketPrice);
            result.ShouldHaveValidationErrorFor(c => c.ReducedTicketPrice);
        }
    }
}