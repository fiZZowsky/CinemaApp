using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Persistance;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace CinemaApp.Infrastructure.Repositories
{
    public class StripeRepository : IStripeRepository
    {
        private readonly StripeSettings _stripeSettings;
        private readonly CinemaAppDbContext _dbContext;

        public StripeRepository(IOptions<StripeSettings> stripeSettings, CinemaAppDbContext dbContext)
        {
            _stripeSettings = stripeSettings.Value;
            _dbContext = dbContext;
        }

        public async Task CreateCheckoutInDatabase(Domain.Entities.Payment payment)
        {
            _dbContext.Add(payment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Session> CreateStripeSession(Domain.Entities.Ticket ticket, int normalTicketPrice, int reducedTicketPrice, Func<string> successUrl, Func<string> cancelURL)
        {
            string successUrlResult = successUrl.Invoke();
            string cancelUrlResult = cancelURL.Invoke();

            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "pln",
                            UnitAmount = CalculateOrderAmount(ticket, normalTicketPrice, reducedTicketPrice),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Ticket",
                                Description = "Movie"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = successUrlResult,
                CancelUrl = cancelUrlResult
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session;
        }

        private int CalculateOrderAmount(Domain.Entities.Ticket ticket, int normalTicketPrice, int reducedTicketPrice)
        {

            int totalAmount = (ticket.NormalPriceSeats * normalTicketPrice) + (ticket.ReducedPriceSeats * reducedTicketPrice);

            return totalAmount;
        }
    }
}
