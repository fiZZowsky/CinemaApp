using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using FluentAssertions;
using CinemaApp.Application.CinemaApp;
using Moq;
using MediatR;
using CinemaApp.Application.CinemaApp.Queries.GetAllMovies;
using Microsoft.AspNetCore.TestHost;

namespace CinemaApp.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HomeControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact()]
        public async Task Index_ReturnsViewWithExpectedData_ForExistingMovies()
        {
            // arrange
            var movies = new List<MovieDto>()
            {
                new MovieDto()
                {
                    Title = "Movie 1",
                },
                new MovieDto()
                {
                    Title = "Movie 2",
                },
                new MovieDto()
                {
                    Title = "Movie 3",
                }
            };

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllMoviesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(movies);

            var client = _factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(services => services.AddScoped(_ => mediatorMock.Object)))
                .CreateClient();

            // act
            var response = await client.GetAsync("/");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("Explore the World of Big Screen Magic!")
                .And.Contain("Movie 1")
                .And.Contain("Movie 2")
                .And.Contain("Movie 3");
        }

        [Fact()]
        public async Task Index_ReturnsEmptyView_WhenNoMovieExist()
        {
            // arrange
            var movies = new List<MovieDto>();

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllMoviesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(movies);

            var client = _factory
                .WithWebHostBuilder(builder => builder.ConfigureTestServices(services => services.AddScoped(_ => mediatorMock.Object)))
                .CreateClient();

            // act
            var response = await client.GetAsync("/");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("Explore the World of Big Screen Magic!");
            content.Should().NotContain("img class=\"image-item\" src=\"https://via.placeholder.com/640x960\" alt=\"Movie cover\"");
        }
    }
}