namespace DishHunter.Web.Controllers
{
    using System.Net;
	using Microsoft.AspNetCore.Mvc;
	using Services.Data.Interfaces;
	using Services.Data.Models.MenuItem;
	using Services.Data.Models.Excel;
	using Infrastructrure.Extensions;
    using Infrastructrure.Helpers;
	using ViewModels.MenuItem;
	using ViewModels.Menu;
    using static Common.NotificationMessagesConstants;

    public class MenuItemController : ExcelController
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
                bool isOwnerHavingMenu = await menuService.AnyMenuOwnedByOwnerByOwnerIdAsync(ownerId!);
                if (!isOwnerHavingMenu)
                {
                    TempData[ErrorMessage] = "Трябва да сте създали меню, за да можете да добавите артикул!";
                    return RedirectToAction("AddSingle", "Menu");
                }
                MenuItemFormViewModel menuItem = await GetViewModel(ownerId!);
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
                if (!ModelState.IsValid)
                {
                    MenuItemFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                bool isMenuExisting = await menuService.ExistsByIdAsync(model.MenuId);
                if (!isMenuExisting)
                {
                    TempData[ErrorMessage] = "Избраното от Вас меню не съществува!";
                    MenuItemFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                bool isOwnerOwningThisMenu = await menuService
                    .MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(model.MenuId, ownerId!);
                if (!isOwnerOwningThisMenu)
                {					
                    TempData[ErrorMessage] = "Не притежавате това меню и не може да добавите артикул в него!";
                    MenuItemFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }				
                MenuItemPostTransferModel menuItem = await GetTransferModel(model);
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
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите артикули!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isMenuExisting = await menuService.ExistsByIdAsync(model.MenuId);
                if (!isMenuExisting)
                {
                    TempData[ErrorMessage] = "Избраното от Вас меню не съществува!";
                    model.Menus = (await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!))
                        .Select(m => new MenuSelectViewModel()
                        {
                            Id = m.Id,
                            MenuType = m.MenuType
                        });
                    return View(model);
                }
                bool isOwnerOwningThisMenu = await menuService
                    .MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(model.MenuId, ownerId!);
                if (!isOwnerOwningThisMenu)
                {
                    TempData[ErrorMessage] = "Не притежавате това меню и не може да добавите артикул в него!";
                    model.Menus = (await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!))
                        .Select(m => new MenuSelectViewModel()
                        {
                            Id = m.Id,
                            MenuType = m.MenuType
                        });
                    return View(model);
                }
                if (excelFile == null)
                {
                    TempData[ErrorMessage] = "Не сте прикачили файл!";
                    model.Menus = (await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!))
                        .Select(m => new MenuSelectViewModel()
                        {
                            Id = m.Id,
                            MenuType = m.MenuType
                        });
                    return View(model);
                }
                if (!(await IsExcelFile(excelFile!)))
                {
                    TempData[ErrorMessage] = "Невалиден файлов формат!";
                    model.Menus = (await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!))
                        .Select(m => new MenuSelectViewModel()
                        {
                            Id = m.Id,
                            MenuType = m.MenuType
                        });
                    return View(model);
                }
                List<MenuItemExcelTransferModel> excelData = new List<MenuItemExcelTransferModel>();
                using (var stream = excelFile!.OpenReadStream())
                {
                    MenuItemExtractResult extractionResult =
                        await excelService.ExtractMenuItemsFromExcel(stream);
                    if (!extractionResult.IsDataExtracted)
                    {
                        TempData[WarningMessage] = extractionResult.Message;
                        model.Menus = (await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!))
                        .Select(m => new MenuSelectViewModel()
                        {
                            Id = m.Id,
                            MenuType = m.MenuType
                        });
                        return View(model);
                    }
                    excelData = extractionResult.MenuItems!.ToList();

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
                ActionHelper helper = await DeleteEditHelper(id.ToString());
                if (!helper.IsAllowed)
                {
                    TempData[ErrorMessage] = helper.Message;
                    return RedirectToAction(helper.ActionName, helper.ControllerName);
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);               
				MenuItemPostTransferModel menuItemTransferModel = await menuItemService
                    .GetMenuItemForEditByIdAsync(id);
                MenuItemFormViewModel menuItemToEdit = await GetViewModel(ownerId!, menuItemTransferModel);
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
                ActionHelper helper = await DeleteEditHelper(id.ToString());
                if (!helper.IsAllowed)
                {
                    TempData[ErrorMessage] = helper.Message;
                    return RedirectToAction(helper.ActionName, helper.ControllerName);
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isMenuExisting = await menuService.ExistsByIdAsync(model.MenuId);
                if (!isMenuExisting)
                {
                    TempData[ErrorMessage] = "Избраното от Вас меню не съществува!";
                    MenuItemFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                bool isOwnerOwningThisMenu = await menuService
                    .MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(model.MenuId, ownerId!);
                if (!isOwnerOwningThisMenu)
                {
                    TempData[ErrorMessage] = "Не притежавате това меню и не може да добавите артикул в него!";
                    MenuItemFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                MenuItemPostTransferModel menuItemToEdit = await GetTransferModel(model);
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
                ActionHelper helper = await DeleteEditHelper(id.ToString());
                if (!helper.IsAllowed)
                {
                    TempData[ErrorMessage] = helper.Message;
                    return RedirectToAction(helper.ActionName, helper.ControllerName);
                }
                await menuItemService.DeleteMenuItemByIdAsync(id);
				return RedirectToAction("Mine", "MenuItem");
			}
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult>DeleteMany(int id)
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
					TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да изтриете артикули от меню!";
					return RedirectToAction("Become", "Owner");
				}
				string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
				bool isOwnerOwningMenu = await menuService
                    .MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(id, ownerId!);
				if (!isOwnerOwningMenu)
				{
                    TempData[ErrorMessage] = "Трябва да притежавате менюто за да имате право да изтриете в артикулите в него!";
					return RedirectToAction("Mine", "Menu");
				}
                await menuItemService.DeleteMenuItemsByMenuIdAsync(id);
                return RedirectToAction("Details", "Menu", new { id = id });
			}
			catch (Exception)
			{
				return GeneralError();
			}
		}
        [HttpGet]
        public async Task<IActionResult> All([FromQuery]MenuItemQueryViewModel query)
        {
			try
			{
                var toTmQuery = new MenuItemQueryTransferModel()
                {
                    SearchItem = query.SearchItem,
                    Sorting = query.Sorting,
                    MenuItems = query.MenuItems.Select(b => new MenuItemsCardTransferModel()
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Price = b.Price,
                        FoodCategory = b.FoodCategory,
                        Description = b.Description,
                        ImageUrl = b.ImageUrl
                    })
                };
                var newQuery = await menuItemService.GetAllMenuItemsAsCardsAsync(toTmQuery);
                var newViewQuery = new MenuItemQueryViewModel()
                {
                    MenuItems = newQuery.MenuItems.Select(b => new MenuItemsCardViewModel()
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Price = b.Price,
                        FoodCategory = b.FoodCategory,
                        Description = b.Description,
                        ImageUrl = b.ImageUrl
                    })
                };
				return View(newViewQuery);
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

        private async Task<MenuItemFormViewModel> GetViewModel(string ownerId)
        {
            MenuItemFormViewModel menuItem = new MenuItemFormViewModel();
            menuItem.Menus = (await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!))
                .Select(m => new MenuSelectViewModel()
            {
                Id = m.Id,
                MenuType = m.MenuType
            });
            return menuItem;
        }
        private async Task<MenuItemFormViewModel> GetViewModel(string ownerId, MenuItemFormViewModel model)
        {
            MenuItemFormViewModel menuItem = new MenuItemFormViewModel()
            {
                Name = model.Name,
                Description = model.Description,
                FoodCategory = model.FoodCategory,
                Price=model.Price,
                ImageUrl=model.ImageUrl
            };
            menuItem.Menus = (await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!))
                 .Select(m => new MenuSelectViewModel()
                 {
                     Id = m.Id,
                     MenuType = m.MenuType
                 });
            return menuItem;
        }
        private async Task<MenuItemFormViewModel> GetViewModel(string ownerId, MenuItemPostTransferModel model)
        {
            MenuItemFormViewModel menuItem = new MenuItemFormViewModel()
            {
                Name = model.Name,
                Description = model.Description,
                FoodCategory = model.FoodCategory,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                MenuId=model.MenuId
            };
            menuItem.Menus = (await menuService.GetMenusForSelectByOwnerIdAsync(ownerId!))
                 .Select(m => new MenuSelectViewModel()
                 {
                     Id = m.Id,
                     MenuType = m.MenuType
                 });
            return menuItem;
        }
        private async Task<MenuItemPostTransferModel> GetTransferModel(MenuItemFormViewModel model)
        {
            return await Task.Run(() =>
            {
                MenuItemPostTransferModel menuItem = new MenuItemPostTransferModel()
                {
                    Name = WebUtility.HtmlEncode(model.Name),
                    FoodCategory = WebUtility.HtmlEncode(model.FoodCategory),
                    Description = WebUtility.HtmlEncode(model.Description),
                    Price = model.Price,
                    ImageUrl = WebUtility.HtmlEncode(model.ImageUrl),
                    MenuId = model.MenuId
                };
                return menuItem;
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
            bool isMenuItemExisting = await menuItemService.ExistsByIdAsync(idToInd);
            if (!isMenuItemExisting)
            {
                helper.IsAllowed = false;
                helper.Message = "Търсеният от Вас артикул не съществува!";
                helper.ActionName = "All";
                helper.ControllerName = "MenuItem";
                return helper;
            }
            bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
            if (!isUserOwner)
            {
                TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да извършите това действие!";
                helper.IsAllowed = false;
                helper.Message = "Трябва да сте ресторантьор за да имате право да редактирате!";
                helper.ActionName = "Become";
                helper.ControllerName = "Owner";
                return helper;
            }
            string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
            bool isOwnerOwningMenuItem = await menuItemService
                .MenuItemOwnedByOwnerByMenuItemIdAndOwnerIdAsync(idToInd, ownerId!);
            if (!isOwnerOwningMenuItem)
            {
                helper.IsAllowed = false;
                helper.Message = "Трябва да притежавате артикула за да имате право да извършите това действие върху него!";
                helper.ActionName = "Mine";
                helper.ControllerName = "MenuItem";
                return helper;
            }          
            return helper;
        }
    }
}
