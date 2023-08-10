using CinemaApp.Infrastructure.Persistance;
using CinemaApp.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CinemaAppDbContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("CinemaApp")));

            services.AddScoped<CinemaAppSeeder>();
        }
    }
}
