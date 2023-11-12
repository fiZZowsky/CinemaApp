using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaApp.MVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using CinemaApp.Application.CinemaApp;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CinemaApp.Application.CinemaApp.Queries.GetRepertoire;
using CinemaApp.Application.CinemaApp.Queries.GetAgeRatings;
using CinemaApp.Application.CinemaApp.Queries.GetAllHalls.GetAllHalls;
using CinemaApp.Application.CinemaApp.Queries.GetAllMovies;
using Microsoft.AspNetCore.TestHost;
using FluentAssertions;
using System.Net;

namespace CinemaApp.MVC.Controllers.Tests
{
    [TestClass()]
    public class MovieShowControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public MovieShowControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact()]
        public async Task Index_ReturnsViewWithExpectedData()
        {
            // arrange
            var movies = new List<MovieDto>
            {
                new MovieDto { Title = "Movie 1", Genre = "Action" },
                new MovieDto { Title = "Movie 2", Genre = "Drama" },
                new MovieDto { Title = "Movie 3", Genre = "Comedy" }
            };

            var ageRatings = new List<AgeRatingDto>
            {
                new AgeRatingDto { Id = 1, MinimumAge = "PG-13" },
                new AgeRatingDto { Id = 2, MinimumAge = "R" },
                new AgeRatingDto { Id = 3, MinimumAge = "PG" }
            };

            var halls = new List<HallDto>
            {
                new HallDto { Number = 1 },
                new HallDto { Number = 2 },
                new HallDto { Number = 3 }
            };

            var expectedCalendar = new List<DateTime>();
            var today = DateTime.Today;

            for (int i = 0; i < 14; i++)
            {
                expectedCalendar.Add(today.AddDays(i));
            }

            var mediatorMock = new Mock<IMediator>();

            mediatorMock.Setup(m => m.Send(It.IsAny<GetRepertoireQuery>(), default))
                .ReturnsAsync(movies);

            mediatorMock.Setup(m => m.Send(It.IsAny<GetAgeRatingsQuery>(), default))
                .ReturnsAsync(ageRatings);

            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllHallsQuery>(), default))
                .ReturnsAsync(halls);

            var client = _factory
              .WithWebHostBuilder(builder => builder.ConfigureTestServices(services => services.AddScoped(_ => mediatorMock.Object)))
              .CreateClient();

            // act
            var response = await client.GetAsync("/MovieShow/Index");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            content.Should().Contain("Movie 1")
                .And.Contain("Action")
                .And.Contain("Movie 2")
                .And.Contain("Drama")
                .And.Contain("Movie 3")
                .And.Contain("Comedy");
        }
    }
}