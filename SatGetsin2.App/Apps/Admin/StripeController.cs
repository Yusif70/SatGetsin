using Microsoft.AspNetCore.Mvc;
using SatGetsin2.Service.Services.Abstractions;

namespace SatGetsin2.App.Apps.Admin
{
	[Route("api/[controller]")]
	[ApiController]
	public class StripeController : ControllerBase
	{
		private readonly IStripeService _stripeService;

		public StripeController(IStripeService stripeService)
		{
			_stripeService = stripeService;
		}

		//[HttpPost("/addPlans")]
		//public async Task<IActionResult> AddPlans()
		//{
		//	var res = await _stripeService.CreateProducts();
		//	return StatusCode(res.StatusCode);
		//}
	}
}
