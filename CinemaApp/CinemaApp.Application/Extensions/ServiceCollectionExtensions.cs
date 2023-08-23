﻿using CinemaApp.Application.ApplicationUser;
using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Application.CinemaApp.Commands.CreateTicket;
using CinemaApp.Application.CinemaApp.Commands.SendEmailWithAttachement;
using CinemaApp.Application.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaApp.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUserContext, UserContext>();
            services.AddMediatR(typeof(CreateMovieCommand));
            services.AddMediatR(typeof(CreateTicketCommand));
            services.AddMediatR(typeof(SendEmailWithAttachementCommand));

            services.AddAutoMapper(typeof(CinemaAppMappingProfile));
        }
    }
}
