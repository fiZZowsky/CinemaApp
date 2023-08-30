﻿using AutoMapper;
using CinemaApp.Application.CinemaApp;
using CinemaApp.Application.CinemaApp.Commands.EditMovie;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Mappings
{
    public class CinemaAppMappingProfile : Profile
    {
        public CinemaAppMappingProfile()
        {
            CreateMap<Domain.Entities.Ticket, TicketDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.MovieShow.Movie.Title))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.MovieShow.Movie.Language))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.MovieShow.Movie.Duration))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.MovieShow.StartTime))
                .ForMember(dest => dest.HallNumber, opt => opt.MapFrom(src => src.MovieShow.Hall.Number))
                .ForMember(dest => dest.RowNumber, opt => opt.MapFrom(src => src.Seats.Select(seat => seat.RowNumber).ToList()))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.Seats.Select(seat => seat.Number).ToList()));

            CreateMap<TicketDto, Domain.Entities.Ticket>()
                .ForMember(dest => dest.MovieShowId, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.QRCode, opt => opt.Ignore())
                .ForMember(dest => dest.IsScanned, opt => opt.MapFrom(src => false));

            CreateMap<Domain.Entities.MovieShow, MovieDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Movie.Title))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Movie.Genre))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Movie.Country))
                .ForMember(dest => dest.AgeRating, opt => opt.MapFrom(src => src.Movie.AgeRating))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Movie.Language))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Movie.Duration))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Movie.Description))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Movie.ReleaseDate))
                .ForMember(dest => dest.HallNumber, opt => opt.MapFrom(src => src.Hall.Number))
                .ForMember(dest => dest.EncodedTitle, opt => opt.MapFrom(src => src.Movie.EncodedTitle));

            CreateMap<MovieDto, Domain.Entities.MovieShow>()
                .ForMember(dest => dest.Movie, opt => opt.MapFrom(src => new Movie()
                {
                    Title = src.Title,
                    Genre = src.Genre,
                    Country = src.Country,
                    AgeRating = src.AgeRating,
                    Language = src.Language,
                    Duration = src.Duration,
                    Description = src.Description,
                    ReleaseDate = src.ReleaseDate
                }))
                .ForMember(dest => dest.HallId, opt => opt.MapFrom(src => src.HallNumber));

            CreateMap<MovieDto, EditMovieCommand>();

            CreateMap<Domain.Entities.Hall, HallDto>();

            CreateMap<MovieDto, TicketDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.HallNumber, opt => opt.MapFrom(src => src.HallNumber));
        }
    }
}
