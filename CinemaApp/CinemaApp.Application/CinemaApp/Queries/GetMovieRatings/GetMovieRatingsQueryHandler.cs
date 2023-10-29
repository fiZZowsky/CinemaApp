using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetMovieRatings
{
    public class GetMovieRatingsQueryHandler : IRequestHandler<GetMovieRatingsQuery, IEnumerable<RatingDto>>
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public GetMovieRatingsQueryHandler(IRatingRepository ratingRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RatingDto>> Handle(GetMovieRatingsQuery request, CancellationToken cancellationToken)
        {
            var result = await _ratingRepository.GetRatingsByMovieId(request.MovieId);

            var dtos = _mapper.Map<IEnumerable<RatingDto>>(result);

            return dtos;
        }
    }
}
