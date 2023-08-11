namespace DishHunter.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using static Common.NotificationMessagesConstants;
    [Authorize]
    public class BaseController : Controller
    {
        protected IActionResult GeneralError()
        {
            TempData[ErrorMessage] = "Появи се неочаквана грешка! Моля опитайте отново по-късно или се свържете с администратор!";
            return RedirectToAction("Index", "Home");
        }
    }
}
