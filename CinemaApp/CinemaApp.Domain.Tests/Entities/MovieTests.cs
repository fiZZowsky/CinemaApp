using Xunit;
using CinemaApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace CinemaApp.Domain.Entities.Tests
{
    public class MovieTests
    {
        [Fact()]
        public void EncodeTitleTest_ShouldSetEncodedTitle()
        {
            // arrange
            var movie = new Movie();
            movie.Title = "Test Movie";

            // act 
            movie.EncodeTitle();

            // assert
            movie.EncodedTitle.Should().Be("test_movie");
        }

        [Fact()]
        public void EncodeTitleTest_ShouldThrowException_WhenTitleIsNull()
        {
            // arrange
            var movie = new Movie();

            // act 
            Action action = () => movie.EncodeTitle();

            // assert
            action.Invoking(a => a.Invoke())
                .Should().Throw<NullReferenceException>();
        }

        [Fact()]
        public void CountRateTest_ShouldSetRating()
        {
            // arrange
            var movie = new Movie { RatingList = new List<Rating> { new Rating { Id = 1, RateValue = 4 }, new Rating { Id = 2, RateValue = 3 } } };

            // act 
            movie.CountRate();

            // assert
            movie.Rating.Should().Be(3.5);
        }

        [Fact]
        public void CountRateTest_ShouldSetDefaultRatingWhenNoRatings()
        {
            // arrange
            var movie = new Movie();

            // act 
            movie.CountRate();

            // assert
            movie.Rating.Should().Be(0);
        }
    }
}