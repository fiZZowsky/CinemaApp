using CinemaApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Seeders
{
    public class CinemaAppSeeder
    {
        private readonly CinemaAppDbContext _dbContext;

        public CinemaAppSeeder(CinemaAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                if (!_dbContext.Halls.Any())
                {
                    var hall1 = new Domain.Entities.Hall { 
                        Number = 1, 
                        NumberOfRows = 5, 
                        PlacesInARow = 10 
                    };
                    var hall2 = new Domain.Entities.Hall { 
                        Number = 2, 
                        NumberOfRows = 6, 
                        PlacesInARow = 8 
                    };
                    _dbContext.Halls.AddRange(hall1, hall2);
                    await _dbContext.SaveChangesAsync();
                }
                if (!_dbContext.Movies.Any())
                {
                    var movie1 = new Domain.Entities.Movie()
                    {
                        Title = "Film 1",
                        Genre = "Sci-fi",
                        Country = "United States",
                        AgeRating = "12+",
                        Language = "english",
                        Duration = 120,
                        Description = "Example description.",
                        ProductionYear = DateTime.UtcNow,
                        ReleaseDate = DateTime.UtcNow
                    };
                    movie1.EncodeTitle();

                    var movie2 = new Domain.Entities.Movie()
                    {
                        Title = "Film 2",
                        Genre = "Comedy",
                        Country = "Poland",
                        AgeRating = "8+",
                        Language = "polish",
                        Duration = 135,
                        Description = "Example description 2.",
                        ProductionYear = DateTime.UtcNow,
                        ReleaseDate = DateTime.UtcNow
                    };
                    movie2.EncodeTitle();

                    _dbContext.Movies.AddRange(movie1, movie2);
                    await _dbContext.SaveChangesAsync();
                }
                if (!_dbContext.MovieShows.Any())
                {
                    var hall1 = await _dbContext.Halls.FirstOrDefaultAsync(h => h.Number == 1);
                    var hall2 = await _dbContext.Halls.FirstOrDefaultAsync(h => h.Number == 2);
                    var movie1 = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Title == "Film 1");
                    var movie2 = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Title == "Film 2");

                    var movieShow1 = new Domain.Entities.MovieShow
                    {
                        MovieId = movie1.Id,
                        HallId = hall1.Id,
                        StartTime = DateTime.UtcNow.AddDays(1)
                    };
                    var movieShow2 = new Domain.Entities.MovieShow
                    {
                        MovieId = movie2.Id,
                        HallId = hall2.Id,
                        StartTime = DateTime.UtcNow.AddDays(2)
                    };
                    _dbContext.MovieShows.AddRange(movieShow1, movieShow2);
                    await _dbContext.SaveChangesAsync();
                }
                if (!_dbContext.Seats.Any())
                {
                    var hall1 = await _dbContext.Halls.FirstOrDefaultAsync(h => h.Number == 1);
                    var hall2 = await _dbContext.Halls.FirstOrDefaultAsync(h => h.Number == 2);

                    for (int row = 1; row <= hall1.NumberOfRows; row++)
                    {
                        for (int place = 1; place <= hall1.PlacesInARow; place++)
                        {
                            var seat = new Domain.Entities.Seat
                            {
                                Number = place,
                                RowNumber = row,
                                HallId = hall1.Id
                            };
                            _dbContext.Seats.Add(seat);
                        }
                    }

                    for (int row = 1; row <= hall2.NumberOfRows; row++)
                    {
                        for (int place = 1; place <= hall2.PlacesInARow; place++)
                        {
                            var seat = new Domain.Entities.Seat
                            {
                                Number = place,
                                RowNumber = row,
                                HallId = hall2.Id
                            };
                            _dbContext.Seats.Add(seat);
                        }
                    }

                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
