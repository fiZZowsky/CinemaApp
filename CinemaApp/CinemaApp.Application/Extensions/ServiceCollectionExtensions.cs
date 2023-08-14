using CinemaApp.Application.CinemaApp.Commands.CreateCinemaApp;
using CinemaApp.Application.Mappings;
using CinemaApp.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaApp.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateMovieDetailsCommand));
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IMovieShowService, MovieShowService>();
            services.AddScoped<ISeatService, SeatService>();

            services.AddAutoMapper(typeof(CinemaAppMappingProfile));
        }
    }
}
