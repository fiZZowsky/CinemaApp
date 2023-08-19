using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetMovieShow
{
    public class GetMovieShowQuery : IRequest<Domain.Entities.MovieShow>
    {
        public DateTime StartTime { get; set; }
        public int HallNumber { get; set; }

        public GetMovieShowQuery(DateTime startTime, int hallNumber)
        {
            StartTime = startTime;
            HallNumber = hallNumber;
        }
    }
}
