using AutoMapper;
using CinemaApp.Application.ApplicationUser;
using CinemaApp.Application.CinemaApp.Queries.GetTicketByGuid;
using CinemaApp.Domain.Interfaces;
using MediatR;

namespace CinemaApp.Application.CinemaApp.Commands.CreateCheckoutInDatabase
{
    public class CreateCheckoutInDatabaseCommandHandler : IRequestHandler<CreateCheckoutInDatabaseCommand>
    {
        private readonly IStripeRepository _stripeRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserContext _userContext;

        public CreateCheckoutInDatabaseCommandHandler(IStripeRepository stripeRepository, ITicketRepository ticketRepository, IUserContext userContext)
        {
            _stripeRepository = stripeRepository;
            _ticketRepository = ticketRepository;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(CreateCheckoutInDatabaseCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            if (currentUser == null)
            {
                return Unit.Value;
            }

            var ticket = await _ticketRepository.GetTicketByGuid(request.TicketId);

            var payment = new Domain.Entities.Payment
            {
                SessionId = request.SessionId,
                TicketId = ticket.Guid,
                PurchasedById = ticket.PurchasedById,
                PurchaseDate = ticket.PurchaseDate
            };

            await _stripeRepository.CreateCheckoutInDatabase(payment);

            return Unit.Value;
        }
    }
}
