namespace DishHunter.Web.Controllers
{
    using System.Net;
	using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using Services.Data.Models.Brand;
	using ViewModels.Menu;
    using ViewModels.Brand;
	using ViewModels.Restaurant;
	using Infrastructrure.Extensions;
    using static Common.NotificationMessagesConstants;

	public class BrandController : BaseController
    {
        private readonly IBrandService brandService;
        private readonly IRestaurantOwnerService ownerService;
        public BrandController(IBrandService _brandService, IRestaurantOwnerService _ownerService)
        {
            brandService = _brandService;
            ownerService = _ownerService;
		}
        [HttpGet]
        public async Task<IActionResult> All()
        {
            try
            {
                var allBrands = (await brandService.GetAllBrandsAsCardsAsync())
                    .Select(b => new BrandsCardViewModel()
                    {
                        Id = b.Id,
                        BrandName = b.BrandName,
                        LogoUrl = b.LogoUrl,
                        WebsiteUrl = b.WebsiteUrl
                    });
                return View(allBrands);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
				bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
				if (!isUserOwner)
				{
					TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да създадете верига!";
					return RedirectToAction("Become", "Owner");
				}
				BrandFormViewModel brand = new BrandFormViewModel();
                return View(brand);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(BrandFormViewModel brand)
        {
            try
            {
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да създадете верига!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                BrandPostTransferModel serviceModel = new BrandPostTransferModel()
                {
                    BrandName = WebUtility.HtmlEncode(brand.BrandName),
                    LogoUrl = WebUtility.HtmlEncode(brand.LogoUrl),
                    WebsiteUrl = WebUtility.HtmlEncode(brand.WebsiteUrl),
                    Description = WebUtility.HtmlEncode(brand.Description)
                };
                string brandId = await brandService.CreateBrandAsync(ownerId!, serviceModel);
                return RedirectToAction("Details", "Brand", new { id = brandId });
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpGet]
        public async Task<IActionResult>Details(string id)
        {
            try
            {
                var brandDetails = await brandService.GetBrandDetailsByIdAsync(id);
                BrandDetailsViewModel model = new BrandDetailsViewModel()
                {
                    Id=id,
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
            catch (Exception)
            {
                return GeneralError();
            }		
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                bool isBrandExisting = await brandService.ExistsByIdAsync(id);
                if (!isBrandExisting)
                {
                    TempData[ErrorMessage] = "Търсеният от Вас бранд не съществува!";
                    return RedirectToAction("All", "Brand");
                }
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да редактирате!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isOwnerOwningBrand = await brandService.BrandOwnedByOwnerIdAndBrandIdAsync(id, ownerId!);
                if (!isOwnerOwningBrand)
                {
                    TempData[ErrorMessage] = "Трябва да притежавате веригата за да имате право да я редактирате!";
                    return RedirectToAction("Mine", "Brand");
                }
                BrandPostTransferModel brandTransferModel = await brandService.GetBrandForEditByIdAsync(id);
                BrandFormViewModel brandToEdit = new BrandFormViewModel()
                {
                    BrandName = brandTransferModel.BrandName,
                    LogoUrl = brandTransferModel.LogoUrl,
                    WebsiteUrl = brandTransferModel.WebsiteUrl,
                    Description = brandTransferModel.Description
                };
                return View(brandToEdit);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, BrandFormViewModel model)
        {
			try
			{
				bool isBrandExisting = await brandService.ExistsByIdAsync(id);
				if (!isBrandExisting)
				{
					TempData[ErrorMessage] = "Търсеният от Вас бранд не съществува!";
					return RedirectToAction("All", "Brand");
				}
				bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
				if (!isUserOwner)
				{
					TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да редактирате!";
					return RedirectToAction("Become", "Owner");
				}
				string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
				bool isOwnerOwningBrand = await brandService.BrandOwnedByOwnerIdAndBrandIdAsync(id, ownerId!);
				if (!isOwnerOwningBrand)
				{
					TempData[ErrorMessage] = "Трябва да притежавате веригата за да имате право да я редактирате!";
					return RedirectToAction("Mine", "Brand");
				}
                BrandPostTransferModel brandTransferModel = new BrandPostTransferModel()
                {
                    BrandName = WebUtility.HtmlEncode(model.BrandName),
                    LogoUrl = WebUtility.HtmlEncode(model.LogoUrl),
                    WebsiteUrl = WebUtility.HtmlEncode(model.WebsiteUrl),
                    Description = WebUtility.HtmlEncode(model.Description)
                };
                await brandService.EditBrandByIdAsync(id, brandTransferModel);
                return RedirectToAction("Details", "Brand", new { id = id });
			}
			catch (Exception)
			{
				return GeneralError();
			}
		}
        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            try
            {
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да можете да видите тази страница!";
                    return RedirectToAction("Become", "Owner");
                }
				string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                IEnumerable<BrandListViewModel> ownerBrands = (await brandService
                    .GetOwnersBrandsByOwnerIdAsync(ownerId!))
                    .Select(tm => new BrandListViewModel()
                    {
                        Id = tm.Id,
                        BrandName = tm.BrandName,
                        LogoUrl = tm.LogoUrl
                    });
                return View(ownerBrands);
			}
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                bool isBrandExisting = await brandService.ExistsByIdAsync(id);
                if (!isBrandExisting)
                {
                    TempData[ErrorMessage] = "Търсеният от Вас бранд не съществува!";
                    return RedirectToAction("All", "Brand");
                }
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да изтриете!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isOwnerOwningBrand = await brandService.BrandOwnedByOwnerIdAndBrandIdAsync(id, ownerId!);
                if (!isOwnerOwningBrand)
                {
                    TempData[ErrorMessage] = "Трябва да притежавате веригата за да имате право да я изтриете!";
                    return RedirectToAction("Mine", "Brand");
                }
                await brandService.DeleteBrandByIdAsync(id);
                return RedirectToAction("Mine", "Brand");
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
	}
}
