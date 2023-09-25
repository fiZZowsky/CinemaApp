using Stripe.Checkout;

namespace CinemaApp.Domain.Interfaces
{
    public interface IStripeRepository
    {
        Task<Session> CreateStripeSession(Domain.Entities.Ticket ticket, Func<string> successUrl, Func<string> cancelURL);
        Task CreateCheckoutInDatabase(Domain.Entities.Payment payment);
    }
}
