using AutoMapper;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetTicketByUser
{
    public class GetTicketByUserQueryHandler : IRequestHandler<GetTicketByUserQuery, TicketDto>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetTicketByUserQueryHandler(ITicketRepository ticketRepository, IMapper mapper, IUserContext userContext)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<TicketDto> Handle(GetTicketByUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();

            var ticket = await _ticketRepository.GetTicketByUser(currentUser.Id, request.PurchaseDate, request.MovieTitle);
            var ticketDto = _mapper.Map<TicketDto>(ticket);
            return ticketDto;
        }
    }
}
