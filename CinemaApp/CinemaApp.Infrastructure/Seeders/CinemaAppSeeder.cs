using CinemaApp.Domain.Entities;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Seeders
{
    public class CinemaAppSeeder
    {
        private readonly CinemaAppDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CinemaAppSeeder(CinemaAppDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                if(!await _userManager.Users.AnyAsync())
                {
                    var user1 = new IdentityUser
                    {
                        UserName = "admin@example.com",
                        Email = "admin@example.com"
                    };
                    var user2 = new IdentityUser
                    {
                        UserName = "piotrekr852@gmail.com",
                        Email = "piotrekr852@gmail.com"
                    };
                    await _userManager.CreateAsync(user1, "Admin123!");
                    await _userManager.CreateAsync(user2, "User123!");
                }
                if(!await _roleManager.Roles.AnyAsync())
                {
                    var user1 = await _userManager.FindByNameAsync("admin@example.com");
                    var user2 = await _userManager.FindByNameAsync("piotrekr852@gmail.com");

                    var role1 = new IdentityRole
                    {
                        Name = "Admin"
                    };
                    var role2 = new IdentityRole
                    {
                        Name = "User"
                    };
                    await _roleManager.CreateAsync(role1);
                    await _roleManager.CreateAsync(role2);

                    await _userManager.AddToRoleAsync(user1, "Admin");
                    await _userManager.AddToRoleAsync(user2, "User");
                }
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
                if (!_dbContext.AgeRatings.Any())
                {
                    var ageRatings = new List<AgeRating>
                    {
                        new AgeRating { MinimumAge = "From 3 years old" },
                        new AgeRating { MinimumAge = "From 7 years old" },
                        new AgeRating { MinimumAge = "From 12 years old" },
                        new AgeRating { MinimumAge = "From 16 years old" },
                        new AgeRating { MinimumAge = "From 18 years old" }
                    };

                    _dbContext.AgeRatings.AddRange(ageRatings);
                    await _dbContext.SaveChangesAsync();
                }
                if (!_dbContext.Movies.Any())
                {
                    var ageRating12 = await _dbContext.AgeRatings.FirstAsync(ar => ar.MinimumAge == "From 12 years old");
                    var ageRating7 = await _dbContext.AgeRatings.FirstAsync(ar => ar.MinimumAge == "From 7 years old");

                    var movies = new List<Movie>
                    {
                        new Movie
                        {
                            Title = "Film 1",
                            Genre = "Sci-fi",
                            Country = "United States",
                            AgeRatingId = ageRating12.Id,
                            Language = "english",
                            Duration = 120,
                            Description = "Example description.",
                            ReleaseDate = DateTime.Today
                        },
                        new Movie
                        {
                            Title = "Film 2",
                            Genre = "Comedy",
                            Country = "Poland",
                            AgeRatingId = ageRating7.Id,
                            Language = "polish",
                            Duration = 135,
                            Description = "Example description 2.",
                            ReleaseDate = DateTime.Today
                        }
                    };

                    foreach (var movie in movies)
                    {
                        movie.EncodeTitle();
                    }

                    _dbContext.Movies.AddRange(movies);
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
                        StartTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day + 1, 12, 0, 0)
                    };
                    var movieShow2 = new Domain.Entities.MovieShow
                    {
                        MovieId = movie2.Id,
                        HallId = hall2.Id,
                        StartTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day + 2, 15, 0, 0)
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
