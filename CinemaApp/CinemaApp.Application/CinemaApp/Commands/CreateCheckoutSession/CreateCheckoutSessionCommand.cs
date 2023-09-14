using MediatR;
using Stripe.Checkout;

namespace CinemaApp.Application.CinemaApp.Commands.CreateCheckoutSession
{
    public class CreateCheckoutSessionCommand : IRequest<Session>
    {
        public TicketDto TicketDto { get; set; }
        public Func<string> SuccessUrl { get; set; }
        public Func<string> CancelURL { get; set; }

        public CreateCheckoutSessionCommand(TicketDto ticketDto, Func<string> successUrl, Func<string> cancelURL)
        {
            TicketDto = ticketDto;
            SuccessUrl = successUrl;
            CancelURL = cancelURL;
        }
    }
}
