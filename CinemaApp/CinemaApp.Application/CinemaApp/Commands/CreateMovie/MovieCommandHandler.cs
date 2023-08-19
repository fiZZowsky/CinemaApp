using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovie
{
    public class MovieCommandHandler : IRequestHandler<CreateMovieCommand>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieCommandHandler(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = _mapper.Map<Domain.Entities.MovieShow>(request);
            movie.Movie.EncodeTitle();

            await _movieRepository.Create(movie);

            return Unit.Value;
        }
    }
}
