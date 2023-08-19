using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateMovie
{
    public class MovieDetailsCommandHandler : IRequestHandler<CreateMovieDetailsCommand>
    {
        private readonly IMovieDetailsRepository _movieDetailsRepository;
        private readonly IMapper _mapper;

        public MovieDetailsCommandHandler(IMovieDetailsRepository movieDetailsRepository, IMapper mapper)
        {
            _movieDetailsRepository = movieDetailsRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(CreateMovieDetailsCommand request, CancellationToken cancellationToken)
        {
            var movie = _mapper.Map<Domain.Entities.MovieShow>(request);
            movie.Movie.EncodeTitle();

            await _movieDetailsRepository.Create(movie);

            return Unit.Value;
        }
    }
}
