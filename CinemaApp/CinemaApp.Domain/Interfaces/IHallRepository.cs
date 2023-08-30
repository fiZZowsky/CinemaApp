namespace CinemaApp.Domain.Interfaces
{
    public interface IHallRepository
    {
        Task<Domain.Entities.Hall> GetHallByNumber(int number);
    }
}
