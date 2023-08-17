namespace DishHunter.Web.Controllers
{
    using System.Net;
	using Microsoft.AspNetCore.Mvc;
	using Services.Data.Models.MenuItem;
	using Services.Data.Interfaces;
	using Services.Data.Models.Excel;
	using Services.Data.Models.Menu;
	using Infrastructrure.Extensions;
    using Infrastructrure.Helpers;
	using ViewModels.Menu;
	using ViewModels.Brand;
    using ViewModels.MenuItem;
	using static Common.NotificationMessagesConstants;

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
                    TempData[ErrorMessage] = "Избраната от Вас верига не съществува!";
                    var ownerBrands = await brandService.GetBrandsForSelectByOwnerId(ownerId!);
                    model.Brands = ownerBrands.Select(b => new BrandSelectViewModel()
                    {
                        Id = b.Id,
                        BrandName = b.BrandName
                    });
                    return View(model);
                }
                bool isOwnerOwningThisBrand = await brandService
                    .BrandOwnedByOwnerIdAndBrandIdAsync(model.BrandId, ownerId!);
                if (!isOwnerOwningThisBrand)
                {
                    TempData[ErrorMessage] = "Не притежавате тази верига и не може да добавите менюта към нея!";
                    var ownerBrands = await brandService.GetBrandsForSelectByOwnerId(ownerId!);
                    model.Brands = ownerBrands.Select(b => new BrandSelectViewModel()
                    {
                        Id = b.Id,
                        BrandName = b.BrandName
                    });
                    return View(model);
                }
                if (excelFile == null)
                {
                    TempData[ErrorMessage] = "Не сте прикачили файл!";
                    var ownerBrands = await brandService.GetBrandsForSelectByOwnerId(ownerId!);
                    model.Brands = ownerBrands.Select(b => new BrandSelectViewModel()
                    {
                        Id = b.Id,
                        BrandName = b.BrandName
                    });
                    return View(model);
                }
                if (!(await IsExcelFile(excelFile!)))
                {
                    TempData[ErrorMessage] = "Невалиден файлов формат!";
                    var ownerBrands = await brandService.GetBrandsForSelectByOwnerId(ownerId!);
                    model.Brands = ownerBrands.Select(b => new BrandSelectViewModel()
                    {
                        Id = b.Id,
                        BrandName = b.BrandName
                    });
                    return View(model);
                }
                List<MenuExcelTransferModel> excelData = new List<MenuExcelTransferModel>();
                using (var stream = excelFile!.OpenReadStream())
                {
                    MenuExtractResult extractionResult =
                        await excelService.ExtractMenuDataFromExcel(stream);
                    if (!extractionResult.IsDataExtracted)
                    {
                        TempData[WarningMessage] = extractionResult.Message;
                        var ownerBrands = await brandService.GetBrandsForSelectByOwnerId(ownerId!);
                        model.Brands = ownerBrands.Select(b => new BrandSelectViewModel()
                        {
                            Id = b.Id,
                            BrandName = b.BrandName
                        });
                        return View(model);
                    }
                    excelData = extractionResult.Menus!.ToList();
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
                ActionHelper helper = await DeleteEditHelper(id.ToString());
                if (!helper.IsAllowed)
                {
                    TempData[ErrorMessage] = helper.Message;
                    return RedirectToAction(helper.ActionName, helper.ControllerName);
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
				MenuPostTransferModel menuTransferModel = await menuService
					.GetMenuForEditByIdAsync(id);
                MenuFormViewModel viewModel = await GetViewModel(ownerId!, menuTransferModel);
				return View(viewModel);
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
                ActionHelper helper = await DeleteEditHelper(id.ToString());
                if (!helper.IsAllowed)
                {
                    TempData[ErrorMessage] = helper.Message;
                    return RedirectToAction(helper.ActionName, helper.ControllerName);
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
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
                MenuPostTransferModel menuToEdit = await GetTransferModel(model);
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
                ActionHelper helper = await DeleteEditHelper(id.ToString());
                if (!helper.IsAllowed)
                {
                    TempData[ErrorMessage] = helper.Message;
                    return RedirectToAction(helper.ActionName, helper.ControllerName);
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
        private async Task<MenuFormViewModel> GetViewModel(string ownerId, MenuPostTransferModel model)
        {
            MenuFormViewModel menu = new MenuFormViewModel()
            {
                MenuType = model.MenuType,
                FoodType = model.FoodType,
                Description = model.Description,
                BrandId = model.BrandId
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
        protected override async Task<ActionHelper> DeleteEditHelper(string id)
        {
            int idToInd = int.Parse(id);
            ActionHelper helper = new ActionHelper()
            {
                IsAllowed = true,
                Message = null,
                ActionName = null,
                ControllerName = null
            };
            bool isMenuExisting = await menuService.ExistsByIdAsync(idToInd);
            if (!isMenuExisting)
            {
                helper.IsAllowed = false;
                helper.Message = "Търсеното от Вас меню не съществува!";
                helper.ActionName = "All";
                helper.ControllerName = "Menu";
                return helper;
            }
            bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
            if (!isUserOwner)
            {
                helper.IsAllowed = false;
                helper.Message = "Трябва да сте ресторантьор за да имате право да извършите това действие!";
                helper.ActionName = "Become";
                helper.ControllerName = "Owner";
                return helper;
            }
            string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
            bool isOwnerOwningMenu = await menuService
                .MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(idToInd, ownerId!);
            if (!isOwnerOwningMenu)
            {
                helper.IsAllowed = false;
                helper.Message = "Трябва да притежавате менюто за да имате право да извършите това действие върху него!";
                helper.ActionName = "Mine";
                helper.ControllerName = "Menu";
                return helper;
            }
            return helper;
        }
    }
}
