using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Application.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaApp.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateMovieDetailsCommand));

            services.AddAutoMapper(typeof(CinemaAppMappingProfile));
        }
    }
}
