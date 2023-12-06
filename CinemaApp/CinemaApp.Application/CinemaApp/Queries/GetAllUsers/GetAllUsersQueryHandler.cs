using AutoMapper;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetAllUsersQueryHandler(IIdentityRepository identityRepository, IMapper mapper, IUserContext userContext)
        {
            _identityRepository = identityRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();

            if (currentUser != null)
            {
                if (currentUser.IsInRole("Admin"))
                {
                    var users = await _identityRepository.GetAllUsers();
                    var dtos = _mapper.Map<IEnumerable<UserDto>>(users);
                    return dtos;
                }
            }

            return null;
        }
    }
}
