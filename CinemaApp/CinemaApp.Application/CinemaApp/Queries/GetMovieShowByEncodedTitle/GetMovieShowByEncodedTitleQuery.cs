using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetMovieShowByEncodedTitle
{
    public class GetMovieShowByEncodedTitleQuery : IRequest<MovieDto>
    {
        public string EncodedTitle { get; set; }
        public DateTime StartTime { get; set; }

        public GetMovieShowByEncodedTitleQuery(string encodedTitle, DateTime startTime)
        {
            EncodedTitle = encodedTitle;
            StartTime = startTime;
        }
    }
}
