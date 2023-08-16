namespace DishHunter.Web.Controllers
{
    using System.Net;
	using Microsoft.AspNetCore.Mvc;
	using Services.Data.Models.MenuItem;
	using Services.Data.Interfaces;
	using Services.Data.Models.Excel;
	using Services.Data.Models.Menu;
	using Infrastructrure.Extensions;
	using ViewModels.Menu;
	using ViewModels.Brand;
    using ViewModels.MenuItem;
	using static Common.NotificationMessagesConstants;
    using DishHunter.Services.Data;
    using DishHunter.Web.ViewModels.Category;
    using DishHunter.Web.ViewModels.Restaurant;
    using DishHunter.Web.ViewModels.Settlement;
    using DishHunter.Services.Data.Models.Restaurant;

    public class MenuController : ExcelController
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
				bool isUserHavingBrand = await brandService.AnyBrandOwnedByOwnerByOwnerIdAsync(ownerId!);
				if (!isUserHavingBrand)
				{
					TempData[ErrorMessage] = "Трябва да сте създали верига, за да можете да добавите меню!";
					return RedirectToAction("Add", "Brand");
				}
				MenuFormViewModel menu = await GetViewModel(ownerId!);
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
                if (!ModelState.IsValid)
				{
					MenuFormViewModel newModel = await GetViewModel(ownerId!, model);
					return View(newModel);
				}				
				bool isBrandExisting = await brandService.ExistsByIdAsync(model.BrandId);
				if (!isBrandExisting)
				{
					TempData[ErrorMessage] = "Избраната от Вас верига не съществува!";
					MenuFormViewModel newModel = await GetViewModel(ownerId!, model);
					return View(newModel);                 
				}
				bool isOwnerOwningThisBrand = await brandService
					.BrandOwnedByOwnerIdAndBrandIdAsync(model.BrandId, ownerId!);
				if (!isOwnerOwningThisBrand)
				{
                    TempData[ErrorMessage] = "Не притежавате тази верига и не може да добавите меню към нея!";
                    MenuFormViewModel newModel = await GetViewModel(ownerId!, model);
					return View(newModel);                
				}
				List<MenuItemExcelTransferModel> excelData = new List<MenuItemExcelTransferModel>();
				if (excelFile != null)
				{
					if (!(await IsExcelFile(excelFile)))
					{
						TempData[WarningMessage] = "Невалиден файлов формат";
                        MenuFormViewModel newModel = await GetViewModel(ownerId!, model);
						return View(newModel);
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
                                MenuFormViewModel newModel = await GetViewModel(ownerId!, model);
                                return View(newModel);

                            }
							excelData = extractionResult.MenuItems!.ToList();
						}
					}
				}
				MenuPostTransferModel menu = await GetTransferModel(model);
				menu.MenuItems = excelData;
                int menuId = await menuService.CreateMenuAsync(menu);
				return RedirectToAction("Details", "Menu", new { id = menuId });
			}
			catch (Exception)
			{
				return GeneralError();
			}
		}
		[HttpGet]
		public async Task<IActionResult> AddMany()
		{
            try
            {
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите менюта!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isUserHavingBrand = await brandService.AnyBrandOwnedByOwnerByOwnerIdAsync(ownerId!);
                if (!isUserHavingBrand)
                {
                    TempData[ErrorMessage] = "Трябва да сте създали верига, за да можете да добавите менюта!";
                    return RedirectToAction("Add", "Brand");
                }
                MenuExcelFormViewModel model = new MenuExcelFormViewModel()
                {
                    Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId!))
					.Select(b => new BrandSelectViewModel()
                    {
                        Id = b.Id,
                        BrandName = b.BrandName
                    })
                };
                return View(model);
            }
            catch (Exception)
            {
                return GeneralError();
            }
		}
		[HttpPost]
        public async Task<IActionResult> AddMany(MenuExcelFormViewModel model, IFormFile excelFile)
		{
            try
            {
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите менюта!";
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
                    ModelState.AddModelError(nameof(model.BrandId), "Не притежавате тази верига и не може да добавите менюта към нея!");
                }
                bool isFileValid = true;
                if (excelFile == null)
                {
                    TempData[ErrorMessage] = "Не сте прикачили файл!";
                    isFileValid = false;
                }
                if (isFileValid && !(await IsExcelFile(excelFile!)))
                {
                    TempData[ErrorMessage] = "Невалиден файлов формат!";
                    isFileValid = false;
                }
                List<MenuExcelTransferModel> excelData = new List<MenuExcelTransferModel>();
                if (isFileValid)
                {
                    using (var stream = excelFile!.OpenReadStream())
                    {
                        MenuExtractResult extractionResult =
                            await excelService.ExtractMenuDataFromExcel(stream);
                        if (!extractionResult.IsDataExtracted)
                        {
                            TempData[WarningMessage] = extractionResult.Message;
                            isFileValid = false;
                        }
                        excelData = extractionResult.Menus!.ToList();
                    }
                }
                if (!isFileValid || !ModelState.IsValid)
                {
                    var ownerBrands = await brandService.GetBrandsForSelectByOwnerId(ownerId!);
					model.Brands = ownerBrands.Select(b => new BrandSelectViewModel()
					{
						Id = b.Id,
						BrandName = b.BrandName
					});
                    return View(model);
                }
                string message = await menuService.AddMenusByBrandIdAsync(excelData, model.BrandId);
                TempData[SuccessMessage] = message;
                return RedirectToAction("Details", "Brand", new { id = model.BrandId });
            }
            catch (Exception)
            {
                return GeneralError();
            }
		}
		[HttpGet]
        public async Task<IActionResult>Edit(int id)
		{
            try
            {
                bool isMenuExisting = await menuService.ExistsByIdAsync(id);
                if (!isMenuExisting)
                {
                    TempData[ErrorMessage] = "Търсеното от Вас меню не съществува!";
                    return RedirectToAction("All", "Menu");
                }
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да редактирате!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isOwnerOwningMenu = await menuService
                    .MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(id, ownerId!);
                if (!isOwnerOwningMenu)
                {
                    TempData[ErrorMessage] = "Трябва да притежавате менюто за да имате право да го редактирате!";
                    return RedirectToAction("Mine", "Menu");
                }
                var ownerBrands = await brandService.GetBrandsForSelectByOwnerId(ownerId!);
				MenuPostTransferModel menuTransferModel = await menuService
					.GetMenuForEditByIdAsync(id);
				MenuFormViewModel model = new MenuFormViewModel()
				{
					MenuType = menuTransferModel.MenuType,
					FoodType = menuTransferModel.FoodType,
					Description = menuTransferModel.Description,
					BrandId = menuTransferModel.BrandId,
					Brands = ownerBrands.Select(tm => new BrandSelectViewModel()
					{
						Id = tm.Id,
						BrandName = tm.BrandName
					})
				};
				return View(model);
            }
            catch (Exception)
            {
                return GeneralError();
            }
		}
		[HttpPost]
		public async Task<IActionResult>Edit(int id,MenuFormViewModel model)
		{
            try
            {
                bool isMenuExisting = await menuService.ExistsByIdAsync(id);
                if (!isMenuExisting)
                {
                    TempData[ErrorMessage] = "Търсеното от Вас меню не съществува!";
                    return RedirectToAction("All", "Menu");
                }
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да редактирате!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isOwnerOwningMenu = await menuService
                    .MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(id, ownerId!);
                if (!isOwnerOwningMenu)
                {
                    TempData[ErrorMessage] = "Трябва да притежавате менюто за да имате право да го редактирате!";
                    return RedirectToAction("Mine", "Menu");
                }
				MenuPostTransferModel menuToEdit = new MenuPostTransferModel()
				{
					MenuType= WebUtility.HtmlEncode(model.MenuType),
					FoodType= WebUtility.HtmlEncode(model.FoodType),
					Description= WebUtility.HtmlEncode(model.Description),
					BrandId=model.BrandId
				};
				await menuService.EditMenuByIdAsync(id, menuToEdit);
				return RedirectToAction("Details", "Menu", new { id = id });
            }
            catch (Exception)
            {
                return GeneralError();
            }
		}
		[HttpPost]
		public async Task<IActionResult>DeleteSingle(int id)
		{
            try
            {
                bool isMenuExisting = await menuService.ExistsByIdAsync(id);
                if (!isMenuExisting)
                {
                    TempData[ErrorMessage] = "Търсеното от Вас меню не съществува!";
                    return RedirectToAction("All", "Menu");
                }
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да изтриете меню!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isOwnerOwningMenu = await menuService.MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(id, ownerId!);
                if (!isOwnerOwningMenu)
                {
                    TempData[ErrorMessage] = "Трябва да притежавате менюто за да имате право да го изтриете!";
                    return RedirectToAction("Mine", "Menu");
                }
				await menuService.DeleteMenuByIdAsync(id);
                return RedirectToAction("Mine", "Menu");
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
		[HttpPost]
		public async Task<IActionResult> DeleteMany(string brandId)
		{
            try
            {
                bool isBrandExisting = await brandService.ExistsByIdAsync(brandId);
                if (!isBrandExisting)
                {
                    TempData[ErrorMessage] = "Търсената от Вас верига не съществува!";
                    return RedirectToAction("All", "Brand");
                }
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да изтриете менюта!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isOwnerOwningBrand = await brandService
                    .BrandOwnedByOwnerIdAndBrandIdAsync(brandId, ownerId!);
                if (!isOwnerOwningBrand)
                {
                    TempData[ErrorMessage] = "Трябва да притежавате веригата за да имате право да изтриете менютата в нея!";
                    return RedirectToAction("Mine", "Brand");
                }
                await menuService.DeleteMenusByBrandIdAsync(brandId);
                return RedirectToAction("Details", "Brand", new { id = brandId }); 
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
		[HttpGet]
		public async Task<IActionResult> Details(int id)
		{
			try
			{
				DetailsMenuTransferModel tm = await menuService.GetMenuDetailsByIdAsync(id);
				MenuDetailsViewModel model = new MenuDetailsViewModel()
				{
					MenuType = tm.MenuType,
					FoodType = tm.FoodType,
					Description = tm.Description,
					Brand = tm.Brand,
					MenuItems = tm.MenuItems
					.Select(mi => new MenuItemListViewModel()
					{
						Id=mi.Id,
						Name=mi.Name,
						FoodCategory=mi.FoodCategory,
						ImageUrl=mi.ImageUrl
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
				IEnumerable<MenuDetailedListViewModel> ownerMenuItems = (await menuService
					.GetOwnerMenusByOwnerIdAsync(ownerId!))
					.Select(tm => new MenuDetailedListViewModel()
					{
						Id = tm.Id,
						MenuType = tm.MenuType,
						FoodType = tm.FoodType,
						Brand = tm.Brand,
						CountItems = tm.CountItems
					});
				return View(ownerMenuItems);
			}
			catch (Exception)
			{
				return GeneralError();
			}
        }
		private async Task<MenuFormViewModel> GetViewModel(string ownerId)
		{
			MenuFormViewModel menu = new MenuFormViewModel();
			menu.Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId))
				.Select(b => new BrandSelectViewModel()
				{
					Id = b.Id,
					BrandName = b.BrandName
				});
			return menu;
		}
        private async Task<MenuFormViewModel> GetViewModel(string ownerId, MenuFormViewModel model)
        {
            MenuFormViewModel menu = new MenuFormViewModel()
            {
                MenuType = model.MenuType,
                Description = model.Description,
                FoodType = model.FoodType
            };
            menu.Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId))
                 .Select(b => new BrandSelectViewModel()
                 {
                     Id = b.Id,
                     BrandName = b.BrandName
                 });
            return menu;
        }
        private async Task<MenuPostTransferModel> GetTransferModel(MenuFormViewModel model)
        {
            return await Task.Run(() =>
            {
				MenuPostTransferModel menu = new MenuPostTransferModel()
				{
					MenuType = WebUtility.HtmlEncode(model.MenuType),
					FoodType = WebUtility.HtmlEncode(model.FoodType),
					Description = WebUtility.HtmlEncode(model.Description),
					BrandId = model.BrandId
				};
                return menu;
            });
        }
    }
}
