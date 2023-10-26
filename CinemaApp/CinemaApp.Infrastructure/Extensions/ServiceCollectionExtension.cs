using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using CinemaApp.Infrastructure.Repositories;
using CinemaApp.Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
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

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CinemaAppDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            services.Configure<StripeSettings>(configuration.GetSection("StripeSettings"));

            services.AddScoped<CinemaAppSeeder>();

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieShowRepository, MovieShowRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<IHallRepository, HallRepository>();
            services.AddScoped<IStripeRepository, StripeRepository>();
            services.AddScoped<IAgeRatingRepository, AgeRatingRepository>();
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
        }
    }
}
