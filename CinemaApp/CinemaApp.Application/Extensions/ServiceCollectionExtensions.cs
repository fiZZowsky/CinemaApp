using CinemaApp.Application.Mappings;
using CinemaApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaApp.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IMovieDetailsService, MovieDetailsService>();
            services.AddScoped<IMovieShowService, MovieShowService>();

            services.AddAutoMapper(typeof(CinemaAppMappingProfile));
        }
    }
}
