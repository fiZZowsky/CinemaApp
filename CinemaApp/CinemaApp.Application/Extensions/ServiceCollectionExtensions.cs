using CinemaApp.Application.ApplicationUser;
using CinemaApp.Application.CinemaApp.Commands.CreateCheckoutSession;
using CinemaApp.Application.CinemaApp.Commands.CreateMovie;
using CinemaApp.Application.CinemaApp.Commands.CreateMovieShow;
using CinemaApp.Application.CinemaApp.Commands.CreateRating;
using CinemaApp.Application.CinemaApp.Commands.CreateTicket;
using CinemaApp.Application.CinemaApp.Commands.EditMovie;
using CinemaApp.Application.CinemaApp.Commands.EditMovieShow;
using CinemaApp.Application.CinemaApp.Commands.EditTicket;
using CinemaApp.Application.CinemaApp.Commands.SendEmailWithAttachement;
using CinemaApp.Application.CinemaApp.Commands.SendRecoveryPasswordEmail;
using CinemaApp.Application.Mappings;
using FluentValidation;
using FluentValidation.AspNetCore;
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
            services.AddMediatR(typeof(CreateMovieShowCommand));
            services.AddMediatR(typeof(CreateRatingCommand));
            services.AddMediatR(typeof(SendEmailWithAttachementCommand));
            services.AddMediatR(typeof(CreateCheckoutSessionCommand));
            services.AddMediatR(typeof(EditTicketCommand));
            services.AddMediatR(typeof(EditMovieCommand));
            services.AddMediatR(typeof(EditMovieShowCommand));
            services.AddMediatR(typeof(SendRecoveryPasswordEmailCommand));

            services.AddAutoMapper(typeof(CinemaAppMappingProfile));

            services.AddValidatorsFromAssemblyContaining<CreateMovieCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<CreateMovieShowCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<CreateRatingCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<EditMovieShowCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            services.AddValidatorsFromAssemblyContaining<EditMovieCommandValidator>()
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
        }
    }
}
