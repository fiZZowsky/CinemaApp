using AutoMapper;
using CinemaApp.Application.Dtos;

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
                .ForMember(dest => dest.RowRunmer, opt => opt.MapFrom(src => src.Seat.RowNumber))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.Seat.Number));

            CreateMap<TicketDto, Domain.Entities.Ticket>()
                            .ForMember(dest => dest.MovieShow, opt => opt.Ignore())
                            .ForMember(dest => dest.Seat, opt => opt.Ignore())
                            .ForMember(dest => dest.Id, opt => opt.Ignore())
                            .ForMember(dest => dest.MovieShowId, opt => opt.MapFrom(src => 0))
                            .ForMember(dest => dest.SeatId, opt => opt.MapFrom(src => 0))
                            .ForMember(dest => dest.IsScanned, opt => opt.MapFrom(src => false));
        }
    }
}
