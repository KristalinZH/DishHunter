namespace DishHunter.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using ViewModels;
    using static Common.NotificationMessagesConstants;
    public class AdminController : BaseController
    {
        private readonly IAdminService adminService;
        private readonly IRestaurantOwnerService ownerService;
        private readonly IBrandService brandService;
        public AdminController(IAdminService _adminService, 
            IRestaurantOwnerService _ownerService, 
            IBrandService _brandService)
        {
            adminService = _adminService;
            ownerService = _ownerService;
            brandService = _brandService;
        }
        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            try
            {
                var users = (await adminService.GetAllUsersAsync())
                    .Select(tm => new UserListViewModel()
                    {
                        Id = tm.Id,
                        UserName = tm.UserName,
                        UserType = tm.UserType
                    });
                return View(users);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> MakeAdmin(string id)
        {
            try 
            { 
                bool isUserExistring = await adminService.UserExistsByIdAsync(id);
                if (!isUserExistring)
                {
                    TempData[ErrorMessage] = "Този потребител не същестува!";
                    return RedirectToAction("Index", "Home", new {area="Administration"});
                }
                bool isUserAlreadyAdmin = await adminService.IsUserAdminByUserId(id);
                if (isUserAlreadyAdmin)
                {
                    TempData[ErrorMessage] = "Този потребител е администратор!";
					return RedirectToAction("Index", "Home", new { area = "Administration" });
				}
                bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(id);
                if (isUserOwner)
                {
                    string? ownerId = await ownerService.GetOwnerIdByUserId(id);
                    await brandService.DeleteBrandsByOwnerIdAsync(ownerId!);
                }
                await adminService.MakeUserAdminAsyncByUserId(id);
				return RedirectToAction("Index", "Home", new { area = "Administration" });
			}
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult>Delete(string id)
        {
            bool isUserExistring = await adminService.UserExistsByIdAsync(id);
            if (!isUserExistring)
            {
                TempData[ErrorMessage] = "Този потребител не същестува!";
				return RedirectToAction("Index", "Home", new { area = "Administration" });
			}
            bool isUserAlreadyAdmin = await adminService.IsUserAdminByUserId(id);
            if (isUserAlreadyAdmin)
            {
                TempData[ErrorMessage] = "Този потребител е администратор!";
				return RedirectToAction("Index", "Home", new { area = "Administration" });
			}
            bool isUserOwner = await ownerService.OwnerExistsByUserIdAsync(id);
            if (isUserOwner)
            {
                string? ownerId = await ownerService.GetOwnerIdByUserId(id);
                await brandService.DeleteBrandsByOwnerIdAsync(ownerId!);
            }
            TempData[SuccessMessage] = "Успешно изтрихте този потребител";
            await adminService.DeleteUserByIdAsync(id);
			return RedirectToAction("Index", "Home", new { area = "Administration" });
		}
    }
}
