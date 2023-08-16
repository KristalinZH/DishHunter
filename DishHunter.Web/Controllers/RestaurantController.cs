namespace DishHunter.Web.Controllers
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using Services.Data.Models.Restaurant;
    using Services.Data.Models.Excel;
    using Infrastructrure.Extensions;
    using Infrastructrure.Helpers;
    using ViewModels.Brand;
    using ViewModels.Restaurant;
    using ViewModels.Category;
    using ViewModels.Settlement;
    using static Common.NotificationMessagesConstants;

    public class RestaurantController : ExcelController
    {
        private readonly IRestaurantService restaurantService;
        private readonly IRestaurantOwnerService ownerService;
        private readonly IBrandService brandService;
        private readonly ICategoryService categoryService;
        private readonly ISettlementService settlementService;
        private readonly IExcelService excelService;
        public RestaurantController(IRestaurantService _restaurantService,
            IRestaurantOwnerService _ownerService,
            IBrandService _brandService,
            IExcelService _excelService,
            ICategoryService _categoryService,
            ISettlementService _settlementService)
        {
            restaurantService = _restaurantService;
            ownerService = _ownerService;
            brandService = _brandService;
            excelService = _excelService;
            categoryService = _categoryService;
            settlementService = _settlementService;
        }
        [HttpGet]
        public async Task<IActionResult> AddSingle()
        {
            try
            {
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да редактирате!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isUserHavingBrand = await brandService.AnyBrandOwnedByOwnerByOwnerIdAsync(ownerId!);
                if (!isUserHavingBrand)
                {                  
                    TempData[ErrorMessage] = "Трябва да сте създали верига, за да можете да добавите ресторант!";
                    return RedirectToAction("Add", "Brand");
                }
                RestaurantFormViewModel restaurant = await GetViewModel(ownerId!);
                return View(restaurant);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddSingle(RestaurantFormViewModel model)
        {
            try
            {
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите ресторант!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                if (!ModelState.IsValid)
                {
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                bool isSettlementExisting = await settlementService.ExistsByIdAsync(model.SettlementId);
                if (!isSettlementExisting)
                {
                    TempData[ErrorMessage] = "Избраната от Вас населено място не съществува!";
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                bool isCategoryExisting = await categoryService.ExistsByIdAsync(model.CategoryId);
                if (!isCategoryExisting)
                {
                    TempData[ErrorMessage] = "Избраната от Вас категория не съществува!";
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                bool isBrandExisting = await brandService.ExistsByIdAsync(model.BrandId);
                if (!isBrandExisting)
                {
                    TempData[ErrorMessage] = "Избраната от Вас верига не съществува!";                   
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                bool isOwnerOwningThisBrand = await brandService
                    .BrandOwnedByOwnerIdAndBrandIdAsync(model.BrandId, ownerId!);
                if (!isOwnerOwningThisBrand)
                {
                    TempData[ErrorMessage] = "Не притежавате тази верига и не може да добавите ресторант към нея!";
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                RestaurantPostTransferModel restaurant = await GetTransferModel(model);
                var status = await restaurantService.CreateRestaurantAsync(restaurant);
                if (!status.IsRestaurantAdded)
                {
                    TempData[ErrorMessage] = status.Message;
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                TempData[SuccessMessage] = status.Message;
                return RedirectToAction("Details", "Restaurant", new { id = status.RestaurantId });
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
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите ресторант!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isUserHavingBrand = await brandService.AnyBrandOwnedByOwnerByOwnerIdAsync(ownerId!);
                if (!isUserHavingBrand)
                {
                    TempData[ErrorMessage] = "Трябва да сте създали верига, за да можете да добавите ресторант!";
                    return RedirectToAction("Add", "Brand");
                }
                RestaurantFormViewModel restaurant = await GetViewModel(ownerId!);
                return View(restaurant);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddMany(RestaurantExcelFormViewModel model,IFormFile excelFile)
        {
            try
            {
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да добавите ресторант!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isBrandExisting = await brandService.ExistsByIdAsync(model.BrandId);
                if (!isBrandExisting)
                {
                    TempData[ErrorMessage] = "Избраната от Вас верига не съществува!";
                    model.Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId!))
                    .Select(b => new BrandSelectViewModel()
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
                    TempData[ErrorMessage] = "Не притежавате тази верига и не може да добавите ресторант към към нея!";
                    model.Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId!))
                    .Select(b => new BrandSelectViewModel()
                    {
                        Id = b.Id,
                        BrandName = b.BrandName
                    });
                    return View(model);
                }
                if (excelFile == null)
                {
                    TempData[ErrorMessage] = "Не сте прикачили файл!";
                    model.Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId!))
                    .Select(b => new BrandSelectViewModel()
                    {
                        Id = b.Id,
                        BrandName = b.BrandName
                    });
                    return View(model);
                }
                bool isExcelFile = await IsExcelFile(excelFile!);
                if (!isExcelFile)
                {
                    TempData[ErrorMessage] = "Невалиден файлов формат!";
                    model.Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId!))
                    .Select(b => new BrandSelectViewModel()
                    {
                        Id = b.Id,
                        BrandName = b.BrandName
                    });
                    return View(model);
                }
                List<RestaurantExcelTransferModel> excelData = new List<RestaurantExcelTransferModel>();
                using (var stream = excelFile!.OpenReadStream())
                {
                    RestaurantExtractResult extractionResult =
                        await excelService.ExtractRestaurantsFromExcel(stream);
                    if (!extractionResult.IsDataExtracted)
                    {
                        TempData[WarningMessage] = extractionResult.Message;
                        model.Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId!))
                        .Select(b => new BrandSelectViewModel()
                        {
                            Id = b.Id,
                            BrandName = b.BrandName
                        });
                        return View(model);
                    }
                    excelData = extractionResult.Restaurants!.ToList();
                }
                var status = await restaurantService.AddRestaurantsByBrandIdAsync(excelData, model.BrandId);
                if (!status.AreRestaurantsAddedSuccessfully)
                {
                    TempData[ErrorMessage] = status.Message;
                    model.Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId!))
                        .Select(b => new BrandSelectViewModel()
                        {
                            Id = b.Id,
                            BrandName = b.BrandName
                        });
                    return View(model);
                }
                TempData[SuccessMessage] = status.Message;
                return RedirectToAction("Details", "Brand", new { id = model.BrandId });
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
                ActionHelper helper = await DeleteEditHelper(id);
                if (!helper.IsAllowed)
                {
                    TempData[ErrorMessage] = helper.Message;
                    return RedirectToAction(helper.ActionName, helper.ControllerName);
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                RestaurantPostTransferModel transferModel = await restaurantService.GetRestaurantForEditByIdAsync(id);
                RestaurantFormViewModel viewModel = await GetViewModel(ownerId!, transferModel);
                return View(viewModel);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, RestaurantFormViewModel model)
        {
            try
            {
                ActionHelper helper = await DeleteEditHelper(id);
                if (!helper.IsAllowed)
                {
                    TempData[ErrorMessage] = helper.Message;
                    return RedirectToAction(helper.ActionName, helper.ControllerName);
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                if (!ModelState.IsValid)
                {
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(model);
                }
                bool isSettlementExisting = await settlementService.ExistsByIdAsync(model.SettlementId);
                if (!isSettlementExisting)
                {
                    TempData[ErrorMessage] = "Избраната от Вас населено място не съществува!";
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                bool isCategoryExisting = await categoryService.ExistsByIdAsync(model.CategoryId);
                if (!isCategoryExisting)
                {
                    TempData[ErrorMessage] = "Избраната от Вас категория не съществува!";
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                bool isBrandExisting = await brandService.ExistsByIdAsync(model.BrandId);
                if (!isBrandExisting)
                {
                    TempData[ErrorMessage] = "Избраната от Вас верига не съществува!";
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                bool isOwnerOwningThisBrand = await brandService
                    .BrandOwnedByOwnerIdAndBrandIdAsync(model.BrandId, ownerId!);
                if (!isOwnerOwningThisBrand)
                {
                    TempData[ErrorMessage] = "Не притежавате тази верига и не може да добавите ресторант към нея!";
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                RestaurantPostTransferModel restaurant = await GetTransferModel(model);
                var status = await restaurantService.CreateRestaurantAsync(restaurant);
                if (!status.IsRestaurantAdded)
                {
                    TempData[ErrorMessage] = status.Message;
                    RestaurantFormViewModel newModel = await GetViewModel(ownerId!, model);
                    return View(newModel);
                }
                TempData[SuccessMessage] = status.Message;
                return RedirectToAction("Details", "Restaurant", new { id = status.RestaurantId });
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSingle(string id)
        {
            try
            {
                ActionHelper helper = await DeleteEditHelper(id);
                if (!helper.IsAllowed)
                {
                    TempData[ErrorMessage] = helper.Message;
                    return RedirectToAction(helper.ActionName, helper.ControllerName);
                }
                await restaurantService.DeleteRestaurantByIdAsync(id);
                return RedirectToAction("Mine", "Menu");
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMany(string id)
        {
            try
            {
                bool isBrandExisting = await brandService.ExistsByIdAsync(id);
                if (!isBrandExisting)
                {
                    TempData[ErrorMessage] = "Избраната от Вас верига не съществува!";
                    return RedirectToAction("All","Brand");
                }
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(User.GetId()!);
                if (!isUserOwner)
                {
                    TempData[ErrorMessage] = "Трябва да сте ресторантьор за да имате право да изтриете ресторанти!";
                    return RedirectToAction("Become", "Owner");
                }
                string? ownerId = await ownerService.GetOwnerIdByUserId(User.GetId()!);
                bool isOwnerOwningThisBrand = await brandService
                    .BrandOwnedByOwnerIdAndBrandIdAsync(id, ownerId!);
                if (!isOwnerOwningThisBrand)
                {
                    TempData[ErrorMessage] = "Не притежавате тази верига и не може да изтриете ресторантите към нея!";
                    return RedirectToAction("Mine", "Brand");
                }               
                await restaurantService.DeleteRestaurantsByBrandIdAsync(id);
                return RedirectToAction("Details", "Brand", new { id = id });
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
                DetailsRestaurantTransferModel transferModel = await restaurantService
                    .GetRestaurantDetailsByIdAsync(id);
                RestaurantDetailsViewModel viewModel = new RestaurantDetailsViewModel()
                {
                    Name=transferModel.Name,
                    PhoneNumber=transferModel.PhoneNumber,
                    ImageUrl=transferModel.ImageUrl,
                    Category=transferModel.Category,
                    Address=transferModel.Address,
                    Settlement=transferModel.Settlement,
                    Region=transferModel.Region,
                    Brand=transferModel.Brand,
                    Latitude=transferModel.Latitude,
                    Longitude=transferModel.Longitude
                };
                return View();
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
                var restaurants = (await restaurantService
                    .GetAllRestaurantsAsCardsAsync())
                    .Select(r => new RestaurantCardViewModel()
                    {
                        Id=r.Id,
                        Name=r.Name,
                        ImageUrl=r.ImageUrl,
                        Brand=r.Brand,
                        Settlement=r.Settlement,
                        Region=r.Settlement
                    });
                return View(restaurants);
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
                var ownerRestaurants = (await restaurantService
                    .GetOwnerRestaurantsByOnwerIdAsync(ownerId!))
                    .Select(tm => new RestaurantListViewModel()
                    {
                        Id = tm.Id,
                        Name = tm.Name,
                        SettlementName = tm.SettlementName
                    });
                return View();
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        private async Task<RestaurantFormViewModel> GetViewModel(string ownerId)
        {
            RestaurantFormViewModel restaurant = new RestaurantFormViewModel();
            restaurant.Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId))
                .Select(b => new BrandSelectViewModel()
                {
                    Id = b.Id,
                    BrandName = b.BrandName
                });
            restaurant.Categories = (await categoryService.AllCategoriesAsync())
                .Select(c => new CategorySelectViewModel()
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName
                });
            restaurant.Settlements = (await settlementService.AllSettlementsAsync())
                .Select(s => new SettlementSelectViewModel()
                {
                    Id = s.Id,
                    SettlementName = s.SettlementName,
                    Region = s.Region
                });
            return restaurant;
        }
        private async Task<RestaurantFormViewModel> GetViewModel(string ownerId,RestaurantFormViewModel model)
        {
            RestaurantFormViewModel restaurant = new RestaurantFormViewModel()
            {
                Name=model.Name,
                PhoneNumber=model.PhoneNumber,
                Address=model.Address,
                ImageUrl=model.ImageUrl
            };
            restaurant.Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId))
                .Select(b => new BrandSelectViewModel()
                {
                    Id = b.Id,
                    BrandName = b.BrandName
                });
            restaurant.Categories = (await categoryService.AllCategoriesAsync())
                .Select(c => new CategorySelectViewModel()
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName
                });
            restaurant.Settlements = (await settlementService.AllSettlementsAsync())
                .Select(s => new SettlementSelectViewModel()
                {
                    Id = s.Id,
                    SettlementName = s.SettlementName,
                    Region = s.Region
                });
            return restaurant;
        }
        private async Task<RestaurantFormViewModel>GetViewModel(string ownerId,RestaurantPostTransferModel model)
        {
            RestaurantFormViewModel restaurant = new RestaurantFormViewModel()
            {
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                ImageUrl = model.ImageUrl,
                BrandId=model.BrandId,
                CategoryId=model.CategoryId,
                SettlementId=model.SettlementId
            };
            restaurant.Brands = (await brandService.GetOwnersBrandsByOwnerIdAsync(ownerId))
                .Select(b => new BrandSelectViewModel()
                {
                    Id = b.Id,
                    BrandName = b.BrandName
                });
            restaurant.Categories = (await categoryService.AllCategoriesAsync())
                .Select(c => new CategorySelectViewModel()
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName
                });
            restaurant.Settlements = (await settlementService.AllSettlementsAsync())
                .Select(s => new SettlementSelectViewModel()
                {
                    Id = s.Id,
                    SettlementName = s.SettlementName,
                    Region = s.Region
                });
            return restaurant;
        }
        private async Task<RestaurantPostTransferModel>GetTransferModel(RestaurantFormViewModel model)
        {
            return await Task.Run(() =>
            {
                RestaurantPostTransferModel restaurant = new RestaurantPostTransferModel()
                {
                    Name = WebUtility.HtmlEncode(model.Name),
                    Address = WebUtility.HtmlEncode(model.Address),
                    ImageUrl = WebUtility.HtmlEncode(model.ImageUrl),
                    PhoneNumber = WebUtility.HtmlEncode(model.PhoneNumber),
                    BrandId = model.BrandId,
                    CategoryId = model.CategoryId,
                    SettlementId = model.SettlementId
                };
                return restaurant;
            });
        }
        protected override async Task<ActionHelper> DeleteEditHelper(string id)
        {
            ActionHelper helper = new ActionHelper()
            {
                IsAllowed = true,
                Message = null,
                ActionName = null,
                ControllerName = null
            };
            bool isRestaurantExisting = await restaurantService.ExistsByIdAsync(id);
            if (!isRestaurantExisting)
            {
                helper.IsAllowed = false;
                helper.Message = "Търсеният от Вас ресторант не съществува!";
                helper.ActionName = "All";
                helper.ControllerName = "Restaurant";
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
            bool isOwnerOwningThisRestaurant = await restaurantService
                .RestaurantOwnedByOwnerByRestaurantIdAndOwnerId(id, ownerId!);
            if (!isOwnerOwningThisRestaurant)
            {
                helper.IsAllowed = false;
                helper.Message = "Трябва да притежавате ресторанта за да имате право да извършите това дейстиве върху него!";
                helper.ActionName = "Mine";
                helper.ControllerName = "Restaurant";
                return helper;
            }
            return helper;            
        }
    }
}
