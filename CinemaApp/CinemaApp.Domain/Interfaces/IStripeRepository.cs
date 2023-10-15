using Stripe.Checkout;

namespace CinemaApp.Domain.Interfaces
{
    public interface IStripeRepository
    {
        Task<Session> CreateStripeSession(Domain.Entities.Ticket ticket, int normalTicketPrice, int reducedTicketPrice, Func<string> successUrl, Func<string> cancelURL);
        Task CreateCheckoutInDatabase(Domain.Entities.Payment payment);
    }
}
