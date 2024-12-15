using Stripe;

namespace SatGetsin2.Service.Services.Concretes
{
    public class StripeService
    {
        private readonly CustomerService _customerService;
        private readonly PaymentIntentService _paymentIntentService;
        private readonly PaymentMethodService _paymentMethodService;
        public StripeService(PaymentIntentService paymentIntentService, CustomerService customerService, PaymentMethodService paymentMethodService)
        {
            _paymentIntentService = paymentIntentService;
            _customerService = customerService;
            _paymentMethodService = paymentMethodService;
        }
        public async Task<Customer> CreateCustomer(string fullName, string email, string paymentMethodId)
        {
            var options = new CustomerCreateOptions
            {
                Name = fullName,
                Email = email,
                PaymentMethod = paymentMethodId,
            };
            var customer = await _customerService.CreateAsync(options);
            return customer;
        }
        public async Task<string> CreatePaymentMethodAsync()
        {
            try
            {
                var options = new PaymentMethodCreateOptions
                {
                    Type = "card",
                    Card = new PaymentMethodCardOptions
                    {
                        Number = "4242424242424242",
                        ExpMonth = 12,
                        ExpYear = 2025,
                        Cvc = "123",
                    },
                };

                var paymentMethod = await _paymentMethodService.CreateAsync(options);
                return paymentMethod.Id;
            }
            catch (StripeException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<(bool Succeeded, string PaymentIntentId, string ErrorMessage)> CreatePaymentIntentAsync(string fullName, string email, double amount, string currency, string paymentMethodId)
        {
            try
            {
                var customer = await CreateCustomer(fullName, email, paymentMethodId);
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(amount * 100),
                    Currency = currency,
                    PaymentMethodTypes = new List<string> { "card" },
                    Description = "Top-up balance",
                    SetupFutureUsage = "off_session",
                    PaymentMethod = paymentMethodId,
                    Customer = customer.Id,
                };

                var paymentIntent = await _paymentIntentService.CreateAsync(options);

                return (paymentIntent.Status == "requires_payment_method", paymentIntent.Id, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
        public async Task<(bool Succeeded, string ErrorMessage)> ConfirmPaymentIntentAsync(string paymentIntentId)
        {
            try
            {
                var paymentIntent = await _paymentIntentService.ConfirmAsync(paymentIntentId);

                if (paymentIntent.Status == "succeeded")
                {
                    return (true, null);
                }

                return (false, paymentIntent.LastPaymentError.Message);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
