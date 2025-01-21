using SatGetsin2.Core.Enums;

namespace SatGetsin2.Service.Dtos.Ad
{
	public class PromoteDto
	{
		public string UserId { get; set; }
		public Guid AdId { get; set; }
		public string BillingName { get; set; }
		public string BillingEmail { get; set; }
		public string PaymentMethod { get; set; }
		public AdLevel Price { get; set; }
		public bool FreeTrial { get; set; }
	}
}
