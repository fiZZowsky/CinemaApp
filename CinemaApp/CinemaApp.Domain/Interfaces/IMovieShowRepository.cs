﻿namespace CinemaApp.Domain.Interfaces
{
    public interface IMovieShowRepository
    {
        Task<IEnumerable<Domain.Entities.MovieShow>> GetAll();
        Task Create(Domain.Entities.MovieShow show);
        Task<Domain.Entities.MovieShow> GetByData(DateTime startTime, int hallNumber);
        Task<bool> IsHallBusy(int hallId, DateTime startTime);
    }
}
