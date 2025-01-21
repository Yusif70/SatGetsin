using Microsoft.AspNetCore.Identity;

namespace SatGetsin2.Core.Entities
{
	public class AppUser : IdentityUser
	{
		public string FullName { get; set; }
		public string? Pfp { get; set; }
		public double Balance { get; set; }
		public string StripeCustomerId { get; set; }
		public List<Ad> Ads { get; set; }
		public List<Favorite> Favorites { get; set; }
	}
}
