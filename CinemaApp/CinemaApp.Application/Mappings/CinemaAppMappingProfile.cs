using AutoMapper;
using CinemaApp.Application.CinemaApp;
using CinemaApp.Application.CinemaApp.Commands.EditMovie;
using CinemaApp.Application.CinemaApp.Commands.EditMovieShow;
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
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.Seats.Select(seat => seat.Number).ToList()))
                .ForMember(dest => dest.NormalTicketPrice, opt => opt.MapFrom(src => src.MovieShow.Movie.PriceList.NormalTicketPrice))
                .ForMember(dest => dest.ReducedTicketPrice, opt => opt.MapFrom(src => src.MovieShow.Movie.PriceList.ReducedTicketPrice));

            CreateMap<TicketDto, Domain.Entities.Ticket>()
                .ForMember(dest => dest.MovieShowId, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.QRCode, opt => opt.Ignore())
                .ForMember(dest => dest.IsScanned, opt => opt.MapFrom(src => false));

            CreateMap<Domain.Entities.MovieShow, MovieDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Movie.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Movie.Title))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Movie.Genre))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Movie.Country))
                .ForMember(dest => dest.AgeRatingId, opt => opt.MapFrom(src => src.Movie.AgeRatingId))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Movie.Language))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Movie.Duration))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Movie.Description))
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.Movie.ReleaseDate))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Movie.Rating))
                .ForMember(dest => dest.HallNumber, opt => opt.MapFrom(src => src.Hall.Number))
                .ForMember(dest => dest.NormalTicketPrice, opt => opt.MapFrom(src => src.Movie.PriceList.NormalTicketPrice))
                .ForMember(dest => dest.ReducedTicketPrice, opt => opt.MapFrom(src => src.Movie.PriceList.ReducedTicketPrice))
                .ForMember(dest => dest.EncodedTitle, opt => opt.MapFrom(src => src.Movie.EncodedTitle));

            CreateMap<MovieDto, Domain.Entities.Movie>()
                .ForMember(dest => dest.PriceList, opt => opt.MapFrom(src => new PriceList()
                {
                    NormalTicketPrice = src.NormalTicketPrice,
                    ReducedTicketPrice = src.ReducedTicketPrice
                }));

            CreateMap<Domain.Entities.Movie, MovieDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.NormalTicketPrice, opt => opt.MapFrom(src => src.PriceList.NormalTicketPrice))
                .ForMember(dest => dest.ReducedTicketPrice, opt => opt.MapFrom(src => src.PriceList.ReducedTicketPrice));

            CreateMap<MovieDto, EditMovieShowCommand>();

            CreateMap<MovieDto, EditMovieCommand>()
                .ForMember(dest => dest.NormalTicketPrice, opt => opt.MapFrom(src => src.NormalTicketPrice))
                .ForMember(dest => dest.ReducedTicketPrice, opt => opt.MapFrom(src => src.ReducedTicketPrice));

            CreateMap<Domain.Entities.Hall, HallDto>();

            CreateMap<MovieDto, TicketDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.HallNumber, opt => opt.MapFrom(src => src.HallNumber))
                .ForMember(dest => dest.NormalTicketPrice, opt => opt.MapFrom(src => src.NormalTicketPrice))
                .ForMember(dest => dest.ReducedTicketPrice, opt => opt.MapFrom(src => src.ReducedTicketPrice));

            CreateMap<Domain.Entities.Ticket, TicketCheckDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.MovieShow.Movie.Title))
                .ForMember(dest => dest.MovieDuration, opt => opt.MapFrom(src => src.MovieShow.Movie.Duration))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.MovieShow.StartTime))
                .ForMember(dest => dest.HallNumber, opt => opt.MapFrom(src => src.MovieShow.Hall.Number))
                .ForMember(dest => dest.RowNumber, opt => opt.MapFrom(src => src.Seats.Select(seat => seat.RowNumber).ToList()))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.Seats.Select(seat => seat.Number).ToList()));

            CreateMap<Domain.Entities.AgeRating, AgeRatingDto>();

            CreateMap<RatingDto, Domain.Entities.Rating>();

            CreateMap<Domain.Entities.Rating, RatingDto>();
        }
    }
}
