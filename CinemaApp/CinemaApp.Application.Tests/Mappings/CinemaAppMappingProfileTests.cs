using Microsoft.VisualStudio.TestTools.UnitTesting;
using CinemaApp.Application.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using CinemaApp.Application.ApplicationUser;
using AutoMapper;
using CinemaApp.Application.CinemaApp;
using FluentAssertions;
using CinemaApp.Domain.Entities;
using CinemaApp.Application.CinemaApp.Commands.EditMovie;

namespace CinemaApp.Application.Mappings.Tests
{
    [TestClass()]
    public class CinemaAppMappingProfileTests
    {
        [TestMethod()]
        public void MappingProfile_ShouldMapTicketToTicketDto()
        {
            // arrange
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CinemaAppMappingProfile()));

            var mapper = configuration.CreateMapper();

            var ticket = new Ticket
            {
                Uid = "1",
                MovieShow = new MovieShow
                {
                    Movie = new Movie
                    {
                        Title = "Movie",
                        Language = "english",
                        Duration = 180,
                        PriceList = new PriceList
                        {
                            NormalTicketPrice = 2500,
                            ReducedTicketPrice = 1500
                        }
                    },
                    StartTime = DateTime.Now,
                    Hall = new Hall
                    {
                        Number = 1
                    }
                },
                Seats = new List<Seat>
        {
            new Seat
            {
                RowNumber = 2,
                Number = 3
            }
        },
                NormalPriceSeats = 1,
                ReducedPriceSeats = 0,
                PurchaseDate = DateTime.Now,
                QRCode = Encoding.UTF8.GetBytes("QRCodeData"),
                IsScanned = false
            };

            // act
            var dto = mapper.Map<TicketDto>(ticket);

            // assert
            dto.Should().NotBeNull();
            dto.MovieTitle.Should().Be("Movie");
            dto.Language.Should().Be("english");
            dto.Duration.Should().Be(180);
            dto.StartTime.Should().Be(ticket.MovieShow.StartTime);
            dto.HallNumber.Should().Be(1);
            dto.RowNumber.Should().ContainInOrder(2);
            dto.SeatNumber.Should().ContainInOrder(3);
            dto.NormalPriceSeats.Should().Be(1);
            dto.ReducedPriceSeats.Should().Be(0);
            dto.NormalTicketPrice.Should().Be(2500);
            dto.ReducedTicketPrice.Should().Be(1500);
            dto.PurchaseDate.Should().Be(ticket.PurchaseDate);
            dto.QRCode.Should().BeEquivalentTo(Encoding.UTF8.GetBytes("QRCodeData"));
        }

        [TestMethod()]
        public void MappingProfile_ShouldMapTicketDtoToTicket()
        {
            // arrange
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CinemaAppMappingProfile()));

            var mapper = configuration.CreateMapper();

            var ticketDto = new TicketDto
            {
                Uid = "1",
                MovieTitle = "Movie",
                Language = "english",
                Duration = 180,
                StartTime = DateTime.Now,
                HallNumber = 1,
                RowNumber = new List<int> { 2 },
                SeatNumber = new List<int> { 3 },
                NormalPriceSeats = 1,
                ReducedPriceSeats = 0,
                NormalTicketPrice = 2500,
                ReducedTicketPrice = 1500,
                PurchaseDate = DateTime.Now,
                QRCode = Encoding.UTF8.GetBytes("QRCodeData")
            };

            // act
            var ticket = mapper.Map<Domain.Entities.Ticket>(ticketDto);

            // assert
            ticket.Should().NotBeNull();
            ticket.Uid.Should().Be(ticketDto.Uid);
            ticket.MovieShowId.Should().Be(0);
            ticket.QRCode.Should().BeNull();
            ticket.IsScanned.Should().Be(false);
            ticket.PurchaseDate.Should().Be(ticketDto.PurchaseDate);
        }

        [TestMethod()]
        public void MappingProfile_ShouldMapMovieShowToMovieDto()
        {
            // arrange
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CinemaAppMappingProfile()));

            var mapper = configuration.CreateMapper();

            var movie = new Domain.Entities.Movie
            {
                Id = 1,
                Title = "Movie",
                Genre = "Sci-Fi",
                Country = "United States",
                AgeRatingId = 1,
                Language = "english",
                Duration = 180,
                Description = "Test",
                ReleaseDate = DateTime.Now,
                PriceList = new PriceList
                {
                    NormalTicketPrice = 2500,
                    ReducedTicketPrice = 1500
                }
            };

            var hall = new Domain.Entities.Hall
            {
                Number = 1
            };

            var movieShow = new Domain.Entities.MovieShow
            {
                Movie = movie,
                Hall = hall
            };

            // act
            var movieDto = mapper.Map<MovieDto>(movieShow);

            // assert
            movieDto.Should().NotBeNull();
            movieDto.MovieId.Should().Be(movie.Id);
            movieDto.Title.Should().Be(movie.Title);
            movieDto.Genre.Should().Be(movie.Genre);
            movieDto.Country.Should().Be(movie.Country);
            movieDto.AgeRatingId.Should().Be(movie.AgeRatingId);
            movieDto.Language.Should().Be(movie.Language);
            movieDto.Duration.Should().Be(movie.Duration);
            movieDto.Description.Should().Be(movie.Description);
            movieDto.ReleaseDate.Should().Be(movie.ReleaseDate);
            movieDto.HallNumber.Should().Be(hall.Number);
            movieDto.NormalTicketPrice.Should().Be(movie.PriceList.NormalTicketPrice);
            movieDto.ReducedTicketPrice.Should().Be(movie.PriceList.ReducedTicketPrice);
        }

        [TestMethod()]
        public void MappingProfile_ShouldMapMovieDtoToMovie()
        {
            // arrange
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CinemaAppMappingProfile()));

            var mapper = configuration.CreateMapper();

            var dto = new MovieDto
            {
                NormalTicketPrice = 2500,
                ReducedTicketPrice = 1500,
            };

            // act
            var result = mapper.Map<Domain.Entities.Movie>(dto);

            // assert
            result.Should().NotBeNull();
            result.PriceList.Should().NotBeNull();
            result.PriceList.NormalTicketPrice.Should().Be(dto.NormalTicketPrice);
            result.PriceList.ReducedTicketPrice.Should().Be(dto.ReducedTicketPrice);
        }

        [TestMethod()]
        public void MappingProfile_ShouldMapMovieDtoToEditMovieCommand()
        {
            // arrange
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CinemaAppMappingProfile()));

            var mapper = configuration.CreateMapper();

            var movieDto = new MovieDto
            {
                NormalTicketPrice = 2500,
                ReducedTicketPrice = 1500
            };

            // act
            var editMovieCommand = mapper.Map<EditMovieCommand>(movieDto);

            // assert
            editMovieCommand.Should().NotBeNull();
            editMovieCommand.NormalTicketPrice.Should().Be(movieDto.NormalTicketPrice);
            editMovieCommand.ReducedTicketPrice.Should().Be(movieDto.ReducedTicketPrice);
        }

        [TestMethod()]
        public void MappingProfile_ShouldMapMovieDtoToTicketDto()
        {
            // arrange
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CinemaAppMappingProfile()));

            var mapper = configuration.CreateMapper();

            var movieDto = new MovieDto
            {
                Title = "Movie Title",
                Language = "English",
                Duration = 120,
                StartTime = DateTime.Now,
                HallNumber = 1,
                NormalTicketPrice = 2500,
                ReducedTicketPrice = 1500
            };

            // act
            var ticketDto = mapper.Map<TicketDto>(movieDto);

            // assert
            ticketDto.Should().NotBeNull();
            ticketDto.MovieTitle.Should().Be(movieDto.Title);
            ticketDto.Language.Should().Be(movieDto.Language);
            ticketDto.Duration.Should().Be(movieDto.Duration);
            ticketDto.StartTime.Should().Be(movieDto.StartTime);
            ticketDto.HallNumber.Should().Be(movieDto.HallNumber);
            ticketDto.NormalTicketPrice.Should().Be(movieDto.NormalTicketPrice);
            ticketDto.ReducedTicketPrice.Should().Be(movieDto.ReducedTicketPrice);
        }

        [TestMethod()]
        public void MappingProfile_ShouldMapTicketToTicketCheckDto()
        {
            // arrange
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CinemaAppMappingProfile()));

            var mapper = configuration.CreateMapper();

            var ticket = new Domain.Entities.Ticket
            {
                Uid = "1",
                MovieShow = new Domain.Entities.MovieShow
                {
                    Movie = new Domain.Entities.Movie
                    {
                        Title = "Movie Title",
                        Duration = 120
                    },
                    StartTime = DateTime.Now,
                    Hall = new Domain.Entities.Hall
                    {
                        Number = 1
                    }
                },
                Seats = new List<Domain.Entities.Seat>
                {
                    new Domain.Entities.Seat { RowNumber = 2, Number = 5 },
                }
            };

            // act
            var ticketCheckDto = mapper.Map<TicketCheckDto>(ticket);

            // assert
            ticketCheckDto.Should().NotBeNull();
            ticketCheckDto.Uid.Should().Be(ticket.Uid);
            ticketCheckDto.MovieTitle.Should().Be(ticket.MovieShow.Movie.Title);
            ticketCheckDto.MovieDuration.Should().Be(ticket.MovieShow.Movie.Duration);
            ticketCheckDto.StartTime.Should().Be(ticket.MovieShow.StartTime);
            ticketCheckDto.HallNumber.Should().Be(ticket.MovieShow.Hall.Number);
            ticketCheckDto.RowNumber.Should().ContainInOrder(ticket.Seats.Select(seat => seat.RowNumber).ToArray());
            ticketCheckDto.SeatNumber.Should().ContainInOrder(ticket.Seats.Select(seat => seat.Number).ToArray());
        }
    }
}