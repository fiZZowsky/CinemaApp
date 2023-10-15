using AutoMapper;
using CinemaApp.Domain.Interfaces;
using MediatR;
using Stripe.Checkout;

namespace CinemaApp.Application.CinemaApp.Commands.CreateCheckoutSession
{
    public class CreateCheckoutSessionCommandHandler : IRequestHandler<CreateCheckoutSessionCommand, Session>
    {
        private readonly IMapper _mapper;
        private readonly IStripeRepository _stripeRepository;

        public CreateCheckoutSessionCommandHandler(IMapper mapper, IStripeRepository stripeRepository)
        {
            _mapper = mapper;
            _stripeRepository = stripeRepository;
        }

        public async Task<Session> Handle(CreateCheckoutSessionCommand request, CancellationToken cancellationToken)
        {
            int normalTicketPrice = request.TicketDto.NormalTicketPrice;
            int reducedTicketPrice = request.TicketDto.ReducedTicketPrice;
            var ticket = _mapper.Map<Domain.Entities.Ticket>(request.TicketDto);

            return await _stripeRepository.CreateStripeSession(ticket, normalTicketPrice, reducedTicketPrice, request.SuccessUrl, request.CancelURL);
        }
    }
}
