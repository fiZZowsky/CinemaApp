using AutoMapper;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAgeRatingById
{
    public class GetAgeRatingByIdQueryHandler : IRequestHandler<GetAgeRatingByIdQuery, AgeRatingDto>
    {
        private readonly IAgeRatingRepository _ageRatingRepository;
        private readonly IMapper _mapper;

        public GetAgeRatingByIdQueryHandler(IAgeRatingRepository ageRatingRepository, IMapper mapper)
        {
            _ageRatingRepository = ageRatingRepository;
            _mapper = mapper;
        }

        public async Task<AgeRatingDto> Handle(GetAgeRatingByIdQuery request, CancellationToken cancellationToken)
        {
            var ageRating = await _ageRatingRepository.GetAgeRatingById(request.AgeRatingId);
            var dto = _mapper.Map<AgeRatingDto>(ageRating);

            return dto;
        }
    }
}
