using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetMovieById
{
    public class GetMovieByIdQuery : IRequest<MovieDto>
    {
        public int Id { get; set; }

        public GetMovieByIdQuery(int id)
        {
            Id = id;
        }
    }
}
