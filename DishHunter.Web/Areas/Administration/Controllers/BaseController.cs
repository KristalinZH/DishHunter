namespace DishHunter.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static Common.RolesConstants;
    using static Common.NotificationMessagesConstants;
    [Area("Administration")]
    [Authorize(Roles =AdminRoleName)]
    public class BaseController : Controller
    {
        protected IActionResult GeneralError()
        {
            TempData[ErrorMessage] = "Появи се неочаквана грешка! Моля опитайте отново по-късно или се свържете с администратор!";
            return RedirectToAction("Index", "Home", new {area="Administration"});
        }
    }
}
