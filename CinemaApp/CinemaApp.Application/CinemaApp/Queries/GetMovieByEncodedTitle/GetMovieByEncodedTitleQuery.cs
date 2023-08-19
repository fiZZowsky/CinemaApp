using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetMovieByEncodedTitle
{
    public class GetMovieByEncodedTitleQuery : IRequest<MovieDto>
    {
        public string EncodedTitle { get; set; }

        public GetMovieByEncodedTitleQuery(string encodedTitle)
        {
            EncodedTitle = encodedTitle;
        }
    }
}
