namespace DishHunter.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using static Common.NotificationMessagesConstants;
    using DishHunter.Web.Infrastructrure.Helpers;

    [Authorize]
    public class BaseController : Controller
    {
        protected IActionResult GeneralError()
        {
            TempData[ErrorMessage] = "Появи се неочаквана грешка! Моля опитайте отново по-късно или се свържете с администратор!";
            return RedirectToAction("Index", "Home");
        }
        protected virtual async Task<ActionHelper> DeleteEditHelper(string id)
        {
            return await Task.Run(() => { return new ActionHelper(); });
        }
    }
}
