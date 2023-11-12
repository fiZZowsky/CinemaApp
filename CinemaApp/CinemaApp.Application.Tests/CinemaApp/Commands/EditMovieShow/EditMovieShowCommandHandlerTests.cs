using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using Moq;
using Xunit;

namespace CinemaApp.Application.CinemaApp.Commands.EditMovieShow.Tests
{
    [TestClass()]
    public class EditMovieShowCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_EditMovieShow_WhenUserIsAdmin()
        {
            // Arrange
            var movieShowRepositoryMock = new Mock<IMovieShowRepository>();
            var hallRepositoryMock = new Mock<IHallRepository>();
            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(uc => uc.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", new[] { "Admin" }));

            var editMovieShowCommandHandler = new EditMovieShowCommandHandler(
                movieShowRepositoryMock.Object,
                hallRepositoryMock.Object,
                userContextMock.Object
            );

            var editMovieShowCommand = new EditMovieShowCommand
            {
                Genre = "NewGenre",
                Country = "NewCountry",
                AgeRatingId = 2,
                Language = "NewLanguage",
                Duration = 120,
                Description = "NewDescription",
                ReleaseDate = DateTime.Now,
                StartTime = DateTime.Now,
                HallNumber = 1
            };

            movieShowRepositoryMock.Setup(repo => repo.GetMovieById(It.IsAny<int>()))
                            .ReturnsAsync((int Id) =>
                            {
                                var movieShow = new Domain.Entities.MovieShow
                                {
                                    Id = 1,
                                    Movie = new Domain.Entities.Movie
                                    {
                                        Id = 1,
                                        Title = "ExistingTitle",
                                        Genre = "Genre",
                                        Country = "Country",
                                        AgeRatingId = 1,
                                        Language = "en",
                                        Duration = 180,
                                        Description = "Test",
                                        ReleaseDate = DateTime.Now
                                    },
                                    Hall = new Domain.Entities.Hall
                                    {
                                        Id = 1,
                                        Number = 1
                                    },  
                                    IsActive = true,
                                    StartTime = DateTime.Now
                                };

                                return movieShow;
                            });

            hallRepositoryMock.Setup(repo => repo.GetHallByNumber(It.IsAny<int>()))
                            .ReturnsAsync((int hallNumber) =>
                            {
                                var hall = new Domain.Entities.Hall
                                {
                                    Id = 1,
                                    Number = 1,
                                    NumberOfRows = 10,
                                    PlacesInARow = 8
                                };

                                return hall;
                            });

            // Act
            await editMovieShowCommandHandler.Handle(editMovieShowCommand, CancellationToken.None);

            // Assert
            movieShowRepositoryMock.Verify(repo => repo.Commit(), Times.Once);
        }

        [Fact()]
        public async Task Handle_EditMovieShow_WhenUserIsNotAdmin()
        {
            // Arrange
            var movieShowRepositoryMock = new Mock<IMovieShowRepository>();
            var hallRepositoryMock = new Mock<IHallRepository>();
            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(uc => uc.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", new[] { "User" }));

            var editMovieShowCommandHandler = new EditMovieShowCommandHandler(
                movieShowRepositoryMock.Object,
                hallRepositoryMock.Object,
                userContextMock.Object
            );

            var editMovieShowCommand = new EditMovieShowCommand
            {
                Genre = "NewGenre",
                Country = "NewCountry",
                AgeRatingId = 2,
                Language = "NewLanguage",
                Duration = 120,
                Description = "NewDescription",
                ReleaseDate = DateTime.Now,
                StartTime = DateTime.Now,
                HallNumber = 1
            };

            movieShowRepositoryMock.Setup(repo => repo.GetMovieById(It.IsAny<int>()))
                            .ReturnsAsync((int Id) =>
                            {
                                var movieShow = new Domain.Entities.MovieShow
                                {
                                    Id = 1,
                                    Movie = new Domain.Entities.Movie
                                    {
                                        Id = 1,
                                        Title = "ExistingTitle",
                                        Genre = "Genre",
                                        Country = "Country",
                                        AgeRatingId = 1,
                                        Language = "en",
                                        Duration = 180,
                                        Description = "Test",
                                        ReleaseDate = DateTime.Now
                                    },
                                    Hall = new Domain.Entities.Hall
                                    {
                                        Id = 1,
                                        Number = 1
                                    },
                                    IsActive = true,
                                    StartTime = DateTime.Now
                                };

                                return movieShow;
                            });

            hallRepositoryMock.Setup(repo => repo.GetHallByNumber(It.IsAny<int>()))
                            .ReturnsAsync((int hallNumber) =>
                            {
                                var hall = new Domain.Entities.Hall
                                {
                                    Id = 1,
                                    Number = 1,
                                    NumberOfRows = 10,
                                    PlacesInARow = 8
                                };

                                return hall;
                            });

            // Act
            await editMovieShowCommandHandler.Handle(editMovieShowCommand, CancellationToken.None);

            // Assert
            movieShowRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
        }

        [Fact()]
        public async Task Handle_EditMovie_WhenUserIsNotAuthenticated()
        {
            // Arrange
            var movieShowRepositoryMock = new Mock<IMovieShowRepository>();
            var hallRepositoryMock = new Mock<IHallRepository>();

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns((CurrentUser?)null);

            var editMovieShowCommandHandler = new EditMovieShowCommandHandler(
                movieShowRepositoryMock.Object,
                hallRepositoryMock.Object,
                userContextMock.Object
            );

            var editMovieShowCommand = new EditMovieShowCommand
            {
                Genre = "NewGenre",
                Country = "NewCountry",
                AgeRatingId = 2,
                Language = "NewLanguage",
                Duration = 120,
                Description = "NewDescription",
                ReleaseDate = DateTime.Now,
                StartTime = DateTime.Now,
                HallNumber = 1
            };

            movieShowRepositoryMock.Setup(repo => repo.GetMovieById(It.IsAny<int>()))
                            .ReturnsAsync((int Id) =>
                            {
                                var movieShow = new Domain.Entities.MovieShow
                                {
                                    Id = 1,
                                    Movie = new Domain.Entities.Movie
                                    {
                                        Id = 1,
                                        Title = "ExistingTitle",
                                        Genre = "Genre",
                                        Country = "Country",
                                        AgeRatingId = 1,
                                        Language = "en",
                                        Duration = 180,
                                        Description = "Test",
                                        ReleaseDate = DateTime.Now
                                    },
                                    Hall = new Domain.Entities.Hall
                                    {
                                        Id = 1,
                                        Number = 1
                                    },
                                    IsActive = true,
                                    StartTime = DateTime.Now
                                };

                                return movieShow;
                            });

            hallRepositoryMock.Setup(repo => repo.GetHallByNumber(It.IsAny<int>()))
                            .ReturnsAsync((int hallNumber) =>
                            {
                                var hall = new Domain.Entities.Hall
                                {
                                    Id = 1,
                                    Number = 1,
                                    NumberOfRows = 10,
                                    PlacesInARow = 8
                                };

                                return hall;
                            });

            // Act
            await editMovieShowCommandHandler.Handle(editMovieShowCommand, CancellationToken.None);

            // Assert
            movieShowRepositoryMock.Verify(repo => repo.Commit(), Times.Never);
        }
    }
}