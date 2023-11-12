using Xunit;
using Moq;
using CinemaApp.Domain.Entities;
using FluentAssertions;
using MediatR;
using AutoMapper;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Application.Mappings;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovie.Tests
{
    public class CreateMovieCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_CreateMovie_WhenUserIsAdmin()
        {
            // Arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CinemaAppMappingProfile()));

            var mapper = configuration.CreateMapper();
            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(uc => uc.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", new[] { "Admin" }));

            var createMovieCommandHandler = new CreateMovieCommandHandler(
                movieRepositoryMock.Object,
                mapper,
                userContextMock.Object
            );

            var createMovieCommand = new CreateMovieCommand()
            {
                Title = "Movie",
                Genre = "Sci-Fi",
                Country = "United States",
                AgeRatingId = 1,
                Language = "english",
                Duration = 180,
                Description = "Test",
                ReleaseDate = DateTime.Now,
                NormalTicketPrice = 2500,
                ReducedTicketPrice = 1500
            };

            // Act
            await createMovieCommandHandler.Handle(createMovieCommand, CancellationToken.None);

            // Assert
            movieRepositoryMock.Verify(r => r.Create(It.IsAny<Domain.Entities.Movie>()), Times.Once);
        }

        [Fact()]
        public async Task Handle_DoesntCreateMovie_WhenUserIsNotAuthorized()
        {
            // Arrange
            var command = new CreateMovieCommand()
            {
                Title = "Movie",
                Genre = "Sci-Fi",
                Country = "United States",
                AgeRatingId = 1,
                Language = "english",
                Duration = 180,
                Description = "Test",
                ReleaseDate = DateTime.Now,
                NormalTicketPrice = 2500,
                ReducedTicketPrice = 1500
            };

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", new[] { "User" }));

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();

            var handler = new CreateMovieCommandHandler(movieRepositoryMock.Object, mapperMock.Object, userContextMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            movieRepositoryMock.Verify(m => m.Create(It.IsAny<Movie>()), Times.Never);
        }

        [Fact()]
        public async Task Handle_DoesntCreateMovie_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var command = new CreateMovieCommand()
            {
                Title = "Movie",
                Genre = "Sci-Fi",
                Country = "United States",
                AgeRatingId = 1,
                Language = "english",
                Duration = 180,
                Description = "Test",
                ReleaseDate = DateTime.Now,
                NormalTicketPrice = 2500,
                ReducedTicketPrice = 1500
            };

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns((CurrentUser?)null);

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();

            var handler = new CreateMovieCommandHandler(movieRepositoryMock.Object, mapperMock.Object, userContextMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            movieRepositoryMock.Verify(m => m.Create(It.IsAny<Movie>()), Times.Never);
        }
    }
}
