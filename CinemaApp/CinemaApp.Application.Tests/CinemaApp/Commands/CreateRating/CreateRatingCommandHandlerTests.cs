using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;
using CinemaApp.Application.Mappings;
using FluentAssertions;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.CinemaApp.Commands.CreateRating.Tests
{
    [TestClass()]
    public class CreateRatingCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_CreateRating_WhenUserIsAuthenticated()
        {
            // Arrange
            var ratingRepositoryMock = new Mock<IRatingRepository>();
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CinemaAppMappingProfile()));

            var mapper = configuration.CreateMapper();
            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(uc => uc.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", new[] { "User" }));

            movieRepositoryMock.Setup(mr => mr.GetMovieById(It.IsAny<int>()))
                .ReturnsAsync(new Domain.Entities.Movie());

            var createRatingCommandHandler = new CreateRatingCommandHandler(
                ratingRepositoryMock.Object,
                movieRepositoryMock.Object,
                mapper,
                userContextMock.Object
            );

            var createRatingCommand = new CreateRatingCommand()
            {
                RateValue = 5,
                Comment = "Test",
                MovieId = 1
            };

            // Act
            await createRatingCommandHandler.Handle(createRatingCommand, CancellationToken.None);

            // Assert
            ratingRepositoryMock.Verify(r => r.Create(It.IsAny<Domain.Entities.Rating>()), Times.Once);
        }

        [Fact()]
        public async Task Handle_DoesntCreateRating_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var command = new CreateRatingCommand()
            {
                RateValue = 5,
                Comment = "Test",
                MovieId = 1
            };

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns((CurrentUser?)null);

            var ratingRepositoryMock = new Mock<IRatingRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieRepositoryMock = new Mock<IMovieRepository>();

            var handler = new CreateRatingCommandHandler(ratingRepositoryMock.Object, movieRepositoryMock.Object, mapperMock.Object, userContextMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
            ratingRepositoryMock.Verify(m => m.Create(It.IsAny<Rating>()), Times.Never);
        }
    }
}