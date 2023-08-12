using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using CinemaApp.Infrastructure.Repositories;
using CinemaApp.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaApp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CinemaAppDbContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("CinemaApp")));

            services.AddScoped<CinemaAppSeeder>();

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IMovieDetailsRepository, MovieDetailsRepository>();
        }
    }
}
