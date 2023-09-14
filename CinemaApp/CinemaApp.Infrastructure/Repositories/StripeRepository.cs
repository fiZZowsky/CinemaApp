using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace CinemaApp.Infrastructure.Repositories
{
    public class StripeRepository : IStripeRepository
    {
        private readonly StripeSettings _stripeSettings;

        public StripeRepository(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
        }

        public async Task<Session> CreateStripeSession(Domain.Entities.Ticket ticket, Func<string> successUrl, Func<string> cancelURL)
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
                            UnitAmount = CalculateOrderAmount(ticket.NormalPriceSeats, ticket.ReducedPriceSeats),
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

        private int CalculateOrderAmount(int normalTicketPriceNumber, int reducedTicketPriceNumber)
        {
            int normalTicketPrice = 2500; // Cena normalnych miejsc w groszach
            int reducedTicketPrice = 1500; // Cena miejsc ulgowych w groszach

            int totalAmount = (normalTicketPriceNumber * normalTicketPrice) + (reducedTicketPriceNumber * reducedTicketPrice);

            return totalAmount;
        }
    }
}
