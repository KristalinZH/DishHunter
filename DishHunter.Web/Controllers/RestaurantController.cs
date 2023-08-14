namespace DishHunter.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class RestaurantController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
