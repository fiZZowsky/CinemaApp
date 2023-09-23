namespace CinemaApp.Domain.Interfaces
{
    public interface IHallRepository
    {
        Task<Domain.Entities.Hall> GetHallByNumber(int number);
        Task<IEnumerable<Domain.Entities.Hall>> GetAllHalls();
    }
}
