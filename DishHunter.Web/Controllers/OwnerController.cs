namespace DishHunter.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using Services.Data.Models.RestaurantOwner;
    using Infrastructrure.Extensions;
    using ViewModels.RestaurantOwner;
    using static Common.NotificationMessagesConstants;

    public class OwnerController : BaseController
    {
        private readonly IRestaurantOwnerService ownerService;
        public OwnerController(IRestaurantOwnerService _ownerService)
        {
            ownerService = _ownerService;
        }
        [HttpGet]
        public async Task<IActionResult> Become()
        {
            string userId = User.GetId()!;
            bool isOwnerAlready = false;
            try
            {
                isOwnerAlready = await ownerService.OwnerExistsByUserIdAsync(userId);
            }
            catch (Exception)
            {
                return GeneralError();
            }
            if (isOwnerAlready)
            {
                TempData[ErrorMessage] = "Вие вече сте ресторантьор!";
                return RedirectToAction("Index", "Home");
            }
            RestaurantOwnerFormViewModel model = new RestaurantOwnerFormViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Become(RestaurantOwnerFormViewModel ownerModel)
        {
            string userId = User.GetId()!;
            try
            {
                bool isOwnerAlready = await ownerService.OwnerExistsByUserIdAsync(userId);
                if (isOwnerAlready)
                {
                    TempData[ErrorMessage] = "Вие вече сте ресторантьор!";
                    return RedirectToAction("Index", "Home");
                }
                bool isPhoneExistring = await ownerService.OwnerExistsByPhoneNumberAsync(ownerModel.PhoneNumber);
                if (isPhoneExistring)
                    ModelState.AddModelError(nameof(ownerModel.PhoneNumber),"Този телефонен номер е вече регистриран в системата! Моля използвайте друг!");
                if(!ModelState.IsValid)
					return View(ownerModel);
				RestaurantOwnerPostTransferModel ownerTransferModel = new RestaurantOwnerPostTransferModel()
                {
                    PhoneNumber = ownerModel.PhoneNumber
                };
                await ownerService.CreateOwnerByUserId(userId, ownerTransferModel);
            }
            catch (Exception)
            {
                return GeneralError();
            }
            return RedirectToAction("Add", "Brand");
        }
    }
}
