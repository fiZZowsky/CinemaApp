using AutoMapper;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Queries.GetAllTickets
{
    public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, IEnumerable<TicketDto>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public GetAllTicketsQueryHandler(ITicketRepository ticketRepository, IMapper mapper, IUserContext userContext)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<IEnumerable<TicketDto>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            IEnumerable<Domain.Entities.Ticket> tickets = null;

            if (currentUser != null) 
            {
                if (currentUser.IsInRole("Admin"))
                {
                    tickets = await _ticketRepository.GetAll();
                }
                else
                {
                    tickets = await _ticketRepository.GetAllByUserId(currentUser.Id);
                }
            }
           
            var dtos = _mapper.Map<IEnumerable<TicketDto>>(tickets);

            return dtos;
        }
    }
}
