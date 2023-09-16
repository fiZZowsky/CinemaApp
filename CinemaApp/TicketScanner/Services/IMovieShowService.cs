namespace TicketScanner.Services
{
    public interface IMovieShowService
    {
        Task<List<Entities.MovieShow>> GetMovieShows();
    }
}
