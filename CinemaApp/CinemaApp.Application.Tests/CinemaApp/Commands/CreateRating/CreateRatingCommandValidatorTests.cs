using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaApp.Application.CinemaApp.Commands.CreateRating;
using CinemaApp.Domain.Interfaces;
using Moq;
using FluentValidation.TestHelper;
using FluentAssertions;
using FluentValidation;

namespace CinemaApp.Application.CinemaApp.Commands.CreateRating.Tests
{
    [TestClass()]
    public class CreateRatingCommandValidatorTests
    {
        [TestMethod()]
        public void Validate_WithValidCommand_ShouldNotHaveValidationError()
        {
            // arrange
            var validator = new CreateRatingCommandValidator();

            var command = new CreateRatingCommand()
            {
                RateValue = 5,
                Comment = "Best movie",
                MovieId = 1
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
            var validator = new CreateRatingCommandValidator();

            var command = new CreateRatingCommand()
            {
                RateValue = 0,
                Comment = "",
                MovieId = 0
            };

            // act
            var result = validator.TestValidate(command);

            // assert
            result.ShouldHaveValidationErrorFor(c => c.RateValue);
            result.ShouldHaveValidationErrorFor(c => c.Comment);
            result.ShouldHaveValidationErrorFor(c => c.MovieId);
        }

        [TestMethod()]
        public void Validate_WithAnOutOfRangeRating_ShouldThrowValidationException()
        {
            // arrange
            var validator = new CreateRatingCommandValidator();

            var command = new CreateRatingCommand()
            {
                RateValue = 7,
                Comment = "Test",
                MovieId = 1
            };

            // act
            Action validationAction = () => validator.ValidateAndThrow(command);

            // assert
            validationAction.Should().Throw<FluentValidation.ValidationException>()
                .Where(ex => ex.Errors.Any(e => e.ErrorMessage == "Rate value must be between 0 and 5."));
        }
    }
}
