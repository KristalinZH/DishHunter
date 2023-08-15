namespace DishHunter.Web.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Services.Data.Interfaces;
	using Infrastructrure.Extensions;
	using ViewModels.MenuItem;
	using ViewModels.Menu;
	using DishHunter.Services.Data.Models.MenuItem;
	using DishHunter.Services.Data.Models.Excel;
	using DishHunter.Services.Data;
	using DishHunter.Web.ViewModels.Brand;
    using static Common.NotificationMessagesConstants;
    using DishHunter.Web.ViewModels.Restaurant;

	public class MenuItemController : BaseController
    {
        private readonly IMenuItemService menuItemService;
		private readonly IMenuService menuService;
		private readonly IRestaurantOwnerService ownerService;
        private readonly IExcelService excelService;
		public MenuItemController(IMenuItemService _menuItemService, 
            IMenuService _menuService,
			IRestaurantOwnerService _ownerService,
			IExcelService _excelService)
        {
            menuItemService = _menuItemService;
            menuService = _menuService;
            ownerService = _ownerService;
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
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите артикул!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                var ownerMenus = await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!);
                if (!ownerMenus.Any())
                {
                    TempData[ErrorMessage] = "Трябва да сте създали меню, за да можете да добавите артикул!";
                    return RedirectToAction("AddSingle", "Menu");
				}
				MenuItemFormViewModel menuItem = new MenuItemFormViewModel();
                menuItem.Menus = ownerMenus.Select(m => new MenuSelectViewModel()
                {
                    Id = m.Id,
                    MenuType = m.MenuType
                });
                return View(menuItem);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult>AddSingle(MenuItemFormViewModel model)
        {
            try
            {
				bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
				if (!isUserOwner)
				{
					TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите артикул!";
					return RedirectToAction("Become", "Owner");
				}
				string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isMenuExisting = await menuService.ExistsByIdAsync(model.MenuId);
                if (!isMenuExisting)
                {
                    ModelState.AddModelError(nameof(model.MenuId),"Избраното от Вас меню не съществува!");
                }
                bool isOwnerOwningThisMenu = await menuService
                    .MenuOwnedByOwnerByMenuIdAndOwnerId(model.MenuId, ownerId!);
                if (!isOwnerOwningThisMenu)
                {
					ModelState.AddModelError(nameof(model.MenuId), "Не притежавате това меню и не може да добавите артикул в него!");
				}
				if (!ModelState.IsValid)
				{
					var ownerMenus = await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!);
					model.Menus = ownerMenus.Select(m => new MenuSelectViewModel()
					{
						Id = m.Id,
						MenuType = m.MenuType
					});
                    return View(model);
				}
                MenuItemPostTransferModel menuItem = new MenuItemPostTransferModel()
                {
                    Name = model.Name,
                    FoodCategory = model.FoodCategory,
                    Description = model.Description,
                    Price = model.Price,
                    ImageUrl = model.ImageUrl,
                    MenuId = model.MenuId                  
                };
                int menuItemId = await menuItemService.CreateMenuItemAsync(menuItem);
                return RedirectToAction("Details", "MenuItem", new { id = menuItemId });
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
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите артикул!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                var ownerMenus = await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!);
                if (!ownerMenus.Any())
                {
                    TempData[ErrorMessage] = "Трябва да сте създали меню, за да можете да добавите артикули!";
                    return RedirectToAction("AddSingle", "Menu");
                }
                MenuItemExcelFormViewModel model = new MenuItemExcelFormViewModel()
                {
                    Menus = ownerMenus.Select(m => new MenuSelectViewModel()
                    {
                        Id = m.Id,
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
        [HttpPost]
        public async Task<IActionResult> AddMany(MenuItemExcelFormViewModel model, IFormFile excelFile)
        {
            try
            {
				bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
				if (!isUserOwner)
				{
					TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите артикул!";
					return RedirectToAction("Become", "Owner");
				}
				string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
				bool isMenuExisting = await menuService.ExistsByIdAsync(model.MenuId);
				if (!isMenuExisting)
				{
					ModelState.AddModelError(nameof(model.MenuId), "Избраното от Вас меню не съществува!");
				}
				bool isOwnerOwningThisMenu = await menuService
					.MenuOwnedByOwnerByMenuIdAndOwnerId(model.MenuId, ownerId!);
				if (!isOwnerOwningThisMenu)
				{
					ModelState.AddModelError(nameof(model.MenuId), "Не притежавате това меню и не може да добавите артикул в него!");
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
				List<MenuItemExcelTransferModel> excelData = new List<MenuItemExcelTransferModel>();
                if (isFileValid)
                {
                    using (var stream = excelFile!.OpenReadStream())
                    {
                        MenuItemExtractResult extractionResult =
                            await excelService.ExtractMenuItemsFromExcel(stream);
                        if (!extractionResult.IsDataExtracted)
                        {
                            TempData[WarningMessage] = extractionResult.Message;
                            isFileValid = false;
                        }
                        excelData = extractionResult.MenuItems!.ToList();
                    }
                }
				if (!isFileValid || !ModelState.IsValid)
				{
					var ownerMenus = await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!);
					model.Menus = ownerMenus.Select(m => new MenuSelectViewModel()
					{
						Id = m.Id,
						MenuType = m.MenuType
					});
					return View(model);
				}              
                string message = await menuItemService.AddMenuItemsByMenuIdAsync(excelData, model.MenuId);
                TempData[SuccessMessage] = message;
				return RedirectToAction("Details", "Menu", new { id = model.MenuId });
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
				bool isMenuItemExisting = await menuItemService.ExistsByIdAsync(id);
				if (!isMenuItemExisting)
				{
					TempData[ErrorMessage] = "Търсеният от Вас артикул не съществува!";
					return RedirectToAction("All", "MenuItem");
				}
				bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
				if (!isUserOwner)
				{
					TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да редактирате!";
					return RedirectToAction("Become", "Owner");
				}
				string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
				bool isOwnerOwningMenuItem = await menuItemService
                    .MenuItemOwnedByOwnerByMenuItemIdAndOwnerId(id, ownerId!);
				if (!isOwnerOwningMenuItem)
				{
					TempData[ErrorMessage] = "Трябва да притежавате артикула за да имате право да го редактирате!";
					return RedirectToAction("Mine", "MenuItem");
				}
				var ownerMenus = await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!);
				if (!ownerMenus.Any())
				{
					TempData[ErrorMessage] = "Трябва да сте създали меню, за да можете да добавите артикул!";
					return RedirectToAction("AddSingle", "Menu");
				}
				MenuItemPostTransferModel menuItemTransferModel = await menuItemService
                    .GetMenuItemForEditByIdAsync(id);
                MenuItemFormViewModel menuItemToEdit = new MenuItemFormViewModel()
                {
                    Name = menuItemTransferModel.Name,
                    Price = menuItemTransferModel.Price,
                    Description = menuItemTransferModel.Description,
                    FoodCategory = menuItemTransferModel.FoodCategory,
                    ImageUrl = menuItemTransferModel.ImageUrl,
                    MenuId = menuItemTransferModel.MenuId,
                    Menus = ownerMenus.Select(m => new MenuSelectViewModel()
                    {
                        Id = m.Id,
                        MenuType = m.MenuType
                    })
                };
                return View(menuItemToEdit);
			}
			catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult>Edit(int id, MenuItemFormViewModel model)
        {
            try
            {
				bool isMenuItemExisting = await menuItemService.ExistsByIdAsync(id);
				if (!isMenuItemExisting)
				{
					TempData[ErrorMessage] = "Търсеният от Вас артикул не съществува!";
					return RedirectToAction("All", "MenuItem");
				}
				bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
				if (!isUserOwner)
				{
					TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да редактирате!";
					return RedirectToAction("Become", "Owner");
				}
				string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
				bool isOwnerOwningMenuItem = await menuItemService
					.MenuItemOwnedByOwnerByMenuItemIdAndOwnerId(id, ownerId!);
				if (!isOwnerOwningMenuItem)
				{
					TempData[ErrorMessage] = "Трябва да притежавате артикула за да имате право да го редактирате!";
					return RedirectToAction("Mine", "MenuItem");
				}
                MenuItemPostTransferModel menuItemToEdit = new MenuItemPostTransferModel()
                {
                    Name = model.Name,
                    Price = model.Price,
                    FoodCategory = model.FoodCategory,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                    MenuId = model.MenuId
                };
                await menuItemService.EditMenuItemByIdAsync(id, menuItemToEdit);
                return RedirectToAction("Details", "MenuItem", new { id = id });
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
                DetailsMenuItemTransferModel transferModel = await menuItemService
                    .GetMenuItemDetailsByIdAsync(id);
                MenuItemDetailsViewModel menuItem = new MenuItemDetailsViewModel()
                {
                    Name = transferModel.Name,
                    Price = transferModel.Price,
                    FoodCategory = transferModel.FoodCategory,
                    Description = transferModel.Description,
                    ImageUrl = transferModel.ImageUrl,
                    Brand = transferModel.Brand,
                    Menu = transferModel.Menu
                };
                return View(menuItem);

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
				bool isMenuItemExisting = await menuItemService.ExistsByIdAsync(id);
				if (!isMenuItemExisting)
				{
					TempData[ErrorMessage] = "Търсеният от Вас артикул не съществува!";
					return RedirectToAction("All", "MenuItem");
				}
				bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
				if (!isUserOwner)
				{
					TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да изтриете артикул!";
					return RedirectToAction("Become", "Owner");
				}
				string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
				bool isOwnerOwningMenuItem = await menuItemService.MenuItemOwnedByOwnerByMenuItemIdAndOwnerId(id, ownerId!);
				if (!isOwnerOwningMenuItem)
				{
					TempData[ErrorMessage] = "Трябва да притежавате веригата за да имате право да я изтриете!";
					return RedirectToAction("Mine", "MenuItem");
				}
				await menuItemService.DeleteMenuItemById(id);
				return RedirectToAction("Mine", "MenuItem");
			}
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult>DeleteMany(int menuId)
        {
			try
			{
				bool isMenuExisting = await menuService.ExistsByIdAsync(menuId);
				if (!isMenuExisting)
				{
					TempData[ErrorMessage] = "Търсеното от Вас меню не съществува!";
					return RedirectToAction("All", "Menu");
				}
				bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
				if (!isUserOwner)
				{
					TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да изтриете артикули от меню!";
					return RedirectToAction("Become", "Owner");
				}
				string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
				bool isOwnerOwningMenu = await menuItemService
                    .MenuItemOwnedByOwnerByMenuItemIdAndOwnerId(menuId, ownerId!);
				if (!isOwnerOwningMenu)
				{
                    TempData[ErrorMessage] = "Трябва да притежавате менюто за да имате право да изтриете в артикулите в него!";
					return RedirectToAction("Mine", "Menu");
				}
                await menuItemService.DeleteMenuItemsByMenuIdAsync(menuId);
                return RedirectToAction("Details", "Menu", new { id = menuId });
			}
			catch (Exception)
			{
				return GeneralError();
			}
		}
        [HttpGet]
        public async Task<IActionResult> All()
        {
			try
			{
				var allMenuItems = (await menuItemService.GetAllMenuItemsAsCardsAsync())
					.Select(b => new MenuItemsCardViewModel()
					{
						Id = b.Id,
						Name=b.Name,
                        Price=b.Price,
                        FoodCategory=b.FoodCategory,
                        Description=b.Description,
                        ImageUrl=b.ImageUrl
					});
				return View(allMenuItems);
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
                IEnumerable<MenuItemListViewModel> ownerMenuItems = (await menuItemService
                    .GetOwnersMenuItemsByOwnerIdAsync(ownerId!))
                    .Select(tm => new MenuItemListViewModel()
                    {
                        Id = tm.Id,
                        Name = tm.Name,
                        FoodCategory = tm.FoodCategory,
                        ImageUrl = tm.ImageUrl
                    });
                return View(ownerMenuItems);
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
