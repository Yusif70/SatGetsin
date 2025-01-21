using Microsoft.AspNetCore.Identity;
using SatGetsin2.Core.Entities;
using SatGetsin2.Service.ApiResponses;
using SatGetsin2.Service.Dtos.Ad;
using SatGetsin2.Service.Services.Abstractions;
using Stripe;

namespace SatGetsin2.Service.Services.Concretes
{
	public class StripeService : IStripeService
	{
		private readonly CustomerService _customerService;
		private readonly PaymentMethodService _paymentMethodService;
		private readonly SubscriptionService _subscriptionService;
		private readonly ProductService _productService;
		private readonly PriceService _priceService;
		private readonly UserManager<AppUser> _userManager;
		public StripeService(CustomerService customerService, PaymentMethodService paymentMethodService, UserManager<AppUser> userManager, SubscriptionService subscriptionService, PriceService priceService, ProductService productService)
		{
			_customerService = customerService;
			_paymentMethodService = paymentMethodService;
			_userManager = userManager;
			_subscriptionService = subscriptionService;
			_priceService = priceService;
			_productService = productService;
		}

		public async Task<ApiResponse> ActivateSubscriptionAsync(PromoteDto dto)
		{
			string customerID = await CreateCustomerAsync(dto);
			await UpdateCustomerPaymentMethodAsync(customerID, dto.PaymentMethod);
			return await CreateSubscriptionAsync(dto);
		}
		private async Task<string> CreateCustomerAsync(PromoteDto dto)
		{
			var user = await _userManager.FindByEmailAsync(dto.BillingEmail);
			var existingCustomer = await _customerService.GetAsync(user.StripeCustomerId);
			if (existingCustomer == null)
			{
				var options = new CustomerCreateOptions
				{
					Email = dto.BillingEmail,
					Name = dto.BillingName
				};
				var customer = await _customerService.CreateAsync(options);
				user.StripeCustomerId = customer.Id;
				await _userManager.UpdateAsync(user);
				return customer.Id;
			}
			return existingCustomer.Id;
		}
		private async Task UpdateCustomerPaymentMethodAsync(string customerID, string paymentMethod)
		{
			var attachOptions = new PaymentMethodAttachOptions { Customer = customerID };
			var attachedPaymentMethod = await _paymentMethodService.AttachAsync(paymentMethod, attachOptions);

			var customerOptions = new CustomerUpdateOptions
			{
				InvoiceSettings = new CustomerInvoiceSettingsOptions
				{
					DefaultPaymentMethod = attachedPaymentMethod.Id
				}
			};
			await _customerService.UpdateAsync(customerID, customerOptions);
		}
		private async Task<ApiResponse> CreateSubscriptionAsync(PromoteDto dto)
		{
			var prices = await _productService.ListAsync();
			var price = prices.FirstOrDefault(p => p.Name == dto.Price.ToString());
			var subscriptionOptions = new SubscriptionCreateOptions
			{
				Items = new List<SubscriptionItemOptions> { new() { Price = price.Id } },
				Metadata = new Dictionary<string, string> { { "adId", dto.AdId.ToString() }, { "userId", dto.UserId } },
			};
			try
			{
				var subscription = await _subscriptionService.CreateAsync(subscriptionOptions);
				return new ApiResponse { StatusCode = 201, Data = price.Metadata, Message = "Subscription created successfully." };
			}
			catch (StripeException ex)
			{
				Console.WriteLine($"Failed to create subscription: {ex}");
				return new ApiResponse { StatusCode = 500, Message = ex.Message };
			}
		}
		public async Task<ApiResponse> CancelSubscription(Guid adId)
		{
			var subscriptions = await _subscriptionService.ListAsync();
			var subscription = subscriptions.FirstOrDefault(s => s.Metadata["adId"] == adId.ToString());
			if (subscription == null)
			{
				return new ApiResponse { StatusCode = 404 };
			}
			await _subscriptionService.CancelAsync(subscription.Id);
			return new ApiResponse { StatusCode = 204 };
		}
		public async Task<ApiResponse> GetUpcomingSubscriptionsAsync(TimeSpan remaining)
		{
			var options = new SubscriptionListOptions
			{
				Status = "active"
			};
			var subscriptions = await _subscriptionService.ListAsync(options);
			var upcomingSubscriptions = subscriptions.Where(s => s.CurrentPeriodEnd < DateTime.Now.Add(remaining)).ToList();
			return new ApiResponse { StatusCode = 200, Data = upcomingSubscriptions };
		}
		public async Task<ApiResponse> CreateProducts()
		{
			var productOptions = new ProductCreateOptions
			{
				Name = "Ad Campaign",
				Description = "Ad campaign subscription plan",
			};
			var product = await _productService.CreateAsync(productOptions);

			var priceOptions1 = new PriceCreateOptions
			{
				Nickname = "1",
				UnitAmount = 100,
				Currency = "usd",
				Recurring = new PriceRecurringOptions { Interval = PlanIntervals.Day },
				Product = product.Id,
				Metadata = new Dictionary<string, string> { { "interval", "1" }, { "amount", "1" } }
			};
			await _priceService.CreateAsync(priceOptions1);
			var priceOptions2 = new PriceCreateOptions
			{
				Nickname = "2",
				UnitAmount = 500,
				Currency = "usd",
				Recurring = new PriceRecurringOptions { Interval = PlanIntervals.Week },
				Product = product.Id,
			};
			await _priceService.CreateAsync(priceOptions2);
			var priceOptions3 = new PriceCreateOptions
			{
				Nickname = "3",
				UnitAmount = 1500,
				Currency = "usd",
				Recurring = new PriceRecurringOptions { Interval = PlanIntervals.Month },
				Product = product.Id,
			};
			await _priceService.CreateAsync(priceOptions3);
			return new ApiResponse { StatusCode = 201 };
		}
	}
}

