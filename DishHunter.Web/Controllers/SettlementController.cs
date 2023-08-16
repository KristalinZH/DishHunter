namespace DishHunter.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static Common.RolesConstants;
    [Authorize(Roles = AdminRoleName)]
    public class SettlementController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
