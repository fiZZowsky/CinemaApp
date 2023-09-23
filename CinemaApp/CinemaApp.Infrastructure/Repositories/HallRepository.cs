using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Repositories
{
    public class HallRepository : IHallRepository
    {
        private readonly CinemaAppDbContext _dbContext;

        public HallRepository(CinemaAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Hall>> GetAllHalls()
            => await _dbContext.Halls.ToListAsync();

        public async Task<Hall> GetHallByNumber(int number)
            => await _dbContext.Halls
            .FirstAsync(h => h.Number == number);
    }
}
