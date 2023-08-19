using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Application.CinemaApp.Commands.CreateTicket;
using CinemaApp.Application.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaApp.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateMovieCommand));
            services.AddMediatR(typeof(CreateTicketCommand));

            services.AddAutoMapper(typeof(CinemaAppMappingProfile));
        }
    }
}
