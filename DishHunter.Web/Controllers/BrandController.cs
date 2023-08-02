namespace DishHunter.Web.Controllers
{
    using DishHunter.Web.ViewModels.Brand;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    [AllowAnonymous]
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
            IEnumerable<AllBrandsViewModel> brands = (await brandService.GetAllBrandsAsync())
                .Select(b => new AllBrandsViewModel()
                {
                    BrandName = b.BrandName,
                    LogoUrl = b.LogoUrl,
                    WebsiteUrl = b.WebsiteUrl
                });
            return View(brands);
        }
    }
}
