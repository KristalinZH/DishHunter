namespace DishHunter.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Services.Data.Interfaces;
    using ViewModels.Brand;
	[AllowAnonymous]
	public class HomeController : BaseController
    {
		private readonly IBrandService brandService;

		public HomeController(IBrandService _brandService)
		{
			brandService = _brandService;
        }

		public async Task<IActionResult> Index()
		{
			IEnumerable<BrandsCardViewModel> brands = (await brandService
				.GetTop3BrandsAsCardsAsync())
				.Select(tm => new BrandsCardViewModel()
				{
					Id = tm.Id,
					BrandName = tm.BrandName,
					LogoUrl = tm.LogoUrl,
					WebsiteUrl = tm.WebsiteUrl
				});
            return View(brands);
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