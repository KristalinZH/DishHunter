namespace DishHunter.Web.Controllers
{
	using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
	using ViewModels.Menu;
    using ViewModels.Brand;
	using ViewModels.Restaurant;
    public class BrandController : BaseController
    {
        private readonly IBrandService brandService;
        public BrandController(IBrandService _brandService)
        {
            brandService = _brandService;
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            IEnumerable<BrandsCardViewModel> brands = (await brandService.GetAllBrandsAsCardsAsync())
                .Select(b => new BrandsCardViewModel()
                {
                    BrandName = b.BrandName,
                    LogoUrl = b.LogoUrl,
                    WebsiteUrl = b.WebsiteUrl
                });
            return View(brands);
        }
        [HttpGet]
        public async Task<IActionResult>Details(string id)
        {
            var brandDetails = await brandService.GetBrandDetailsByIdAsync(id);
            BrandDetailsViewModel model = new BrandDetailsViewModel()
            {
                BrandName = brandDetails.BrandName,
                LogoUrl = brandDetails.LogoUrl,
                Description = brandDetails.Description,
                WebsiteUrl = brandDetails.WebsiteUrl,
                Restaurants = brandDetails.Restaurants.Select(r => new RestaurantListViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    SettlementName = r.SettlementName
                }),
                Menus = brandDetails.Menus.Select(m => new MenuListViewModel()
                {
                    Id = m.Id,
                    FoodType = m.FoodType,
                    MenuType = m.MenuType
                })
            };
			return View(model);
        }
    }
}
