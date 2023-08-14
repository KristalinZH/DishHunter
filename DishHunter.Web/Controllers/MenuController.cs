namespace DishHunter.Web.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Services.Data.Models.MenuItem;
	using Services.Data.Interfaces;
	using Services.Data.Models.Excel;
	using Services.Data.Models.Menu;
	using ViewModels.Menu;
	using ViewModels.MenuItem;
	using Infrastructrure.Extensions;
	using ViewModels.Brand;
	using static Common.NotificationMessagesConstants;
	using System.Text;

	public class MenuController : BaseController
    {
		private readonly IBrandService brandService;
		private readonly IMenuService menuService;
		private readonly IRestaurantOwnerService ownerService;
		private readonly IExcelService excelService;
		public MenuController(IMenuService _menuService, 
			IRestaurantOwnerService _ownerService,
			IBrandService _brandService,
			IExcelService _excelService)
        {
			menuService = _menuService;
			ownerService = _ownerService;
			brandService = _brandService;
			excelService = _excelService;
		}
		[HttpGet]
		public async Task<IActionResult> AddSingle()
		{
			try
			{
				bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
				if (!isUserOwner)
				{
					TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите меню!";
					return RedirectToAction("Become", "Owner");
				}
				string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
				var ownerBrands = await brandService.GetBrandsForSelectByOwnerId(ownerId!);
				if (!ownerBrands.Any())
				{
					TempData[ErrorMessage] = "Трябва да сте създали верига, за да можете да добавите меню!";
					return RedirectToAction("Add", "Brand");
				}
				MenuFormViewModel menu = new MenuFormViewModel();
				menu.Brands = ownerBrands.Select(b => new BrandSelectViewModel()
				{
					Id = b.Id,
					BrandName = b.BrandName
				});
				return View(menu);
			}
			catch (Exception)
			{
				return GeneralError();
			}
		}
		[HttpPost]
		public async Task<IActionResult> AddSingle(MenuFormViewModel model,IFormFile? excelFile)
		{
			try
			{
				bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
				if (!isUserOwner)
				{
					TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите меню!";
					return RedirectToAction("Become", "Owner");
				}
				string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
				bool isBrandExisting = await brandService.ExistsByIdAsync(model.BrandId);
				if (!isBrandExisting)
				{
					ModelState.AddModelError(nameof(model.BrandId), "Избраната от Вас верига не съществува!");
				}
				bool isOwnerOwningThisBrand = await brandService
					.BrandOwnedByOwnerIdAndBrandIdAsync(model.BrandId, ownerId!);
				if (!isOwnerOwningThisBrand)
				{
					ModelState.AddModelError(nameof(model.BrandId), "Не притежавате тази верига и не може да добавите меню към нея!");
				}
				if (!ModelState.IsValid)
				{
					var ownerBrands = await brandService.GetBrandsForSelectByOwnerId(ownerId!);
					model.Brands = ownerBrands.Select(m => new BrandSelectViewModel()
					{
						Id = m.Id,
						BrandName = m.BrandName
					});
					return View(model);
				}
				List<MenuItemExcelTransferModel> excelData = new List<MenuItemExcelTransferModel>();
				if (excelFile != null)
				{
					if (!(await IsExcelFile(excelFile)))
					{
						TempData[WarningMessage] = "Невалиден файлов формат";
						var ownerBrands = await brandService.GetBrandsForSelectByOwnerId(ownerId!);
						model.Brands = ownerBrands.Select(m => new BrandSelectViewModel()
						{
							Id = m.Id,
							BrandName = m.BrandName
						});
						return View(model);
					}
					else
					{
						using (var stream = excelFile.OpenReadStream())
						{
							MenuItemExtractResult extractionResult = 
								await excelService.ExtractMenuItemsFromExcel(stream);						
							if (!extractionResult.IsDataExtracted)
							{
								TempData[WarningMessage] = extractionResult.Message;
								var ownerBrands = await brandService.GetBrandsForSelectByOwnerId(ownerId!);
								model.Brands = ownerBrands.Select(m => new BrandSelectViewModel()
								{
									Id = m.Id,
									BrandName = m.BrandName
								});
								return View(model);

							}
							excelData = extractionResult.MenuItems!.ToList();
						}
					}
				}				
				MenuPostTransferModel menu = new MenuPostTransferModel()
				{
					MenuType = model.MenuType,
					FoodType = model.FoodType,
					Description = model.Description,
					BrandId = model.BrandId,
					MenuItems = excelData
				};
				int menuId = await menuService.CreateMenuAsync(menu);
				return RedirectToAction("Details", "Menu", new { id = menuId });
			}
			catch (Exception)
			{
				return GeneralError();
			}
		}
		private async Task<bool> IsExcelFile(IFormFile file)
		{
			return await Task.Run(() =>
			{
				return file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
				|| file.ContentType == "application/vnd.ms-excel"
				|| Path.GetExtension(file.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase)
				|| Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase);
			});
		}
	}
}
