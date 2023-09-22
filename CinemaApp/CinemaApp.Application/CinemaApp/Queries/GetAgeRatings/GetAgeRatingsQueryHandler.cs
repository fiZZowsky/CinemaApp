using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAgeRatings
{
    public class GetAgeRatingsQueryHandler : IRequestHandler<GetAgeRatingsQuery, IEnumerable<AgeRatingDto>>
    {
        private readonly IAgeRatingRepository _ageRatingRepository;
        private readonly IMapper _mapper;

        public GetAgeRatingsQueryHandler(IAgeRatingRepository ageRatingRepository, IMapper mapper)
        {
            _ageRatingRepository = ageRatingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AgeRatingDto>> Handle(GetAgeRatingsQuery request, CancellationToken cancellationToken)
        {
            var ageRatings = await _ageRatingRepository.GetAgeRatings();

            var dto = _mapper.Map<IEnumerable<AgeRatingDto>>(ageRatings);

            return dto;
        }
    }
}
