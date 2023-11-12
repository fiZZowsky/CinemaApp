using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using Moq;
using System;

namespace CinemaApp.Application.CinemaApp.Commands.EditMovie.Tests
{
    [TestClass()]
    public class EditMovieCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_EditMovie_WhenUserIsAdmin()
        {
            // Arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(uc => uc.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", new[] { "Admin" }));

            var editMovieCommandHandler = new EditMovieCommandHandler(
                movieRepositoryMock.Object,
                userContextMock.Object
            );

            var editMovieCommand = new EditMovieCommand
            {
                Title = "Title",
                Genre = "NewGenre",
                Country = "NewCountry",
                AgeRatingId = 2,
                Language = "NewLanguage",
                Duration = 120,
                Description = "NewDescription",
                ReleaseDate = DateTime.Now,
                NormalTicketPrice = 1500,
                ReducedTicketPrice = 1000
            };

            movieRepositoryMock.Setup(repo => repo.GetMovieByEncodedTitle(It.IsAny<string>()))
                            .ReturnsAsync((string encodedTitle) =>
                            {
                                var movie = new Domain.Entities.Movie
                                {
                                    Id = 1,
                                    Title = "ExistingTitle",
                                    Genre = "Genre",
                                    Country = "Country",
                                    AgeRatingId = 1,
                                    Language = "en",
                                    Duration = 180,
                                    Description = "Test",
                                    ReleaseDate = DateTime.Now,
                                    PriceList = new Domain.Entities.PriceList
                                    {
                                        Id = 1,
                                        NormalTicketPrice = 1000,
                                        ReducedTicketPrice = 500
                                    }
                                };

                                return movie;
                            });

            // Act
            await editMovieCommandHandler.Handle(editMovieCommand, CancellationToken.None);

            // Assert
            movieRepositoryMock.Verify(repo => repo.Commit(), Times.Once);
        }

        [Fact()]
        public async Task Handle_EditMovie_WhenUserIsNotAdmin()
        {
            // Arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(uc => uc.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", new[] { "User" }));

            var editMovieCommandHandler = new EditMovieCommandHandler(
                movieRepositoryMock.Object,
                userContextMock.Object
            );

            var editMovieCommand = new EditMovieCommand
            {
                Genre = "NewGenre",
                Country = "NewCountry",
                AgeRatingId = 2,
                Language = "NewLanguage",
                Duration = 120,
                Description = "NewDescription",
                ReleaseDate = DateTime.Now,
                NormalTicketPrice = 1500,
                ReducedTicketPrice = 1000
            };

            movieRepositoryMock.Setup(repo => repo.GetMovieByEncodedTitle(It.IsAny<string>()))
                .ReturnsAsync(new Domain.Entities.Movie() { });

            // Act
            await editMovieCommandHandler.Handle(editMovieCommand, CancellationToken.None);

            // Assert
            movieRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
        }

        [Fact()]
        public async Task Handle_EditMovie_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns((CurrentUser?)null);

            var editMovieCommandHandler = new EditMovieCommandHandler(
                movieRepositoryMock.Object,
                userContextMock.Object
            );

            var editMovieCommand = new EditMovieCommand
            {
                Genre = "NewGenre",
                Country = "NewCountry",
                AgeRatingId = 2,
                Language = "NewLanguage",
                Duration = 120,
                Description = "NewDescription",
                ReleaseDate = DateTime.Now,
                NormalTicketPrice = 1500,
                ReducedTicketPrice = 1000
            };

            movieRepositoryMock.Setup(repo => repo.GetMovieByEncodedTitle(It.IsAny<string>()))
                .ReturnsAsync(new Domain.Entities.Movie() { });

            // Act
            await editMovieCommandHandler.Handle(editMovieCommand, CancellationToken.None);

            // Assert
            movieRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
        }
    }
}