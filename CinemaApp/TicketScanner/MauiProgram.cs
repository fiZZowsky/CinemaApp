using Camera.MAUI;
using Microsoft.Extensions.Configuration;
using TicketScanner.Persistance;
using Microsoft.EntityFrameworkCore;
using TicketScanner.Services;
using TicketScanner.ViewModels;

namespace TicketScanner;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCameraView()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        var connectionString = "Server=DESKTOP-QOHOSVJ;Database=CinemaAppDb;Trusted_Connection=True;TrustServerCertificate=True;";

        builder.Services.AddDbContext<TicketScannerDbContext>(options =>
            options.UseSqlServer(connectionString));

        //Services
        builder.Services.AddScoped<IMovieShowService, MovieShowService>();

        // Views registration
        builder.Services.AddSingleton<MainPage>();

        //View models
        builder.Services.AddSingleton<MovieShowViewModel>();

        return builder.Build();
    }
}
