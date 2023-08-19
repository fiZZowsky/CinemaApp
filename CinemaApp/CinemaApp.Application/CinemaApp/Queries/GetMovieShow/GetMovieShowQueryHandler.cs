using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetMovieShow
{
    public class GetMovieShowQueryHandler : IRequestHandler<GetMovieShowQuery, Domain.Entities.MovieShow>
    {
        private readonly IMovieShowRepository _movieShowRepository;

        public GetMovieShowQueryHandler(IMovieShowRepository movieShowRepository)
        {
            _movieShowRepository = movieShowRepository;
        }

        public async Task<Domain.Entities.MovieShow> Handle(GetMovieShowQuery request, CancellationToken cancellationToken)
        {
            var movieShow = await _movieShowRepository.GetByData(request.StartTime, request.HallNumber);
            return movieShow;
        }
    }
}
