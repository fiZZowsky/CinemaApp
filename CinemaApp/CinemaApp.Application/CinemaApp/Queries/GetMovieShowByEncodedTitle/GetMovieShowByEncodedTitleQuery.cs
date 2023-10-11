using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetMovieShowByEncodedTitle
{
    public class GetMovieShowByEncodedTitleQuery : IRequest<MovieDto>
    {
        public string EncodedTitle { get; set; }

        public GetMovieShowByEncodedTitleQuery(string encodedTitle)
        {
            EncodedTitle = encodedTitle;
        }
    }
}
