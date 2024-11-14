using Stripe;

namespace Advertisement.Service.Services.Concretes
{
    public class PaymentService
    {
        private readonly PaymentIntentService _paymentIntentService;

        public PaymentService(PaymentIntentService paymentIntentService)
        {
            _paymentIntentService = paymentIntentService;
        }
        public async Task<bool> ProcessPaymentAsync(string userId, double amount, string currency = "azn")
        {
            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100),
                    Currency = currency,
                    PaymentMethodTypes = new List<string> { "card" },
                    Customer = userId,
                };
                var paymentIntent = await _paymentIntentService.CreateAsync(options);
                //var confirmOptions = new PaymentIntentConfirmOptions
                //{
                //};
                var confirmedPaymentIntent = await _paymentIntentService.ConfirmAsync(paymentIntent.Id);

                return confirmedPaymentIntent.Status == "succeeded";
            }
            catch (StripeException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
