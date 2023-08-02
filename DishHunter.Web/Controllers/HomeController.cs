namespace DishHunter.Web.Controllers
{
	using System.Diagnostics;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
	using ViewModels;
	[AllowAnonymous]
	public class HomeController : BaseController
    {
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(int statuscode)
		{
			if (statuscode == 400 || statuscode == 404)
			{
				return View("Error404");
			}
			if (statuscode == 401)
			{
				return View("Error401");
			}
			return View();
		}
	}
}