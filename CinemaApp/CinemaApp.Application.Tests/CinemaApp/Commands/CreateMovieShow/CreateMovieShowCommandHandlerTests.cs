using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;
using CinemaApp.Domain.Entities;
using FluentAssertions;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovieShow.Tests
{
    [TestClass()]
    public class CreateMovieShowCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_CreateMovieShow_WhenUserIsAdmin()
        {
            // Arrange
            var movieShowRepositoryMock = new Mock<IMovieShowRepository>();
            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(uc => uc.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", new[] { "Admin" }));

            var createMovieShowCommandHandler = new CreateMovieShowCommandHandler(
                movieShowRepositoryMock.Object,
                userContextMock.Object
            );

            var createMovieShowCommand = new CreateMovieShowCommand()
            {
                MovieId = 1,
                HallId = 1,
                StartTime = DateTime.Now,
                IsActive = false
            };

            // Act
            await createMovieShowCommandHandler.Handle(createMovieShowCommand, CancellationToken.None);

            // Assert
            movieShowRepositoryMock.Verify(r => r.Create(It.IsAny<Domain.Entities.MovieShow>()), Times.Once);
        }

        [Fact()]
        public async Task Handle_DoesntCreateMovieShow_WhenUserIsNotAuthorized()
        {
            // Arrange
            var command = new CreateMovieShowCommand()
            {
                MovieId = 1,
                HallId = 1,
                StartTime = DateTime.Now,
                IsActive = false
            };

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", new[] { "User" }));

            var movieShowRepositoryMock = new Mock<IMovieShowRepository>();

            var handler = new CreateMovieShowCommandHandler(movieShowRepositoryMock.Object, userContextMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            movieShowRepositoryMock.Verify(m => m.Create(It.IsAny<MovieShow>()), Times.Never);
        }

        [Fact()]
        public async Task Handle_DoesntCreateMovieShow_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var command = new CreateMovieShowCommand()
            {
                MovieId = 1,
                HallId = 1,
                StartTime = DateTime.Now,
                IsActive = false
            };

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns((CurrentUser?)null);

            var movieShowRepositoryMock = new Mock<IMovieShowRepository>();

            var handler = new CreateMovieShowCommandHandler(movieShowRepositoryMock.Object, userContextMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            movieShowRepositoryMock.Verify(m => m.Create(It.IsAny<MovieShow>()), Times.Never);
        }
    }
}