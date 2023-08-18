namespace DishHunter.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Interfaces;
    using Services.Data.Models.Category;
    using ViewModels.Category;
    using static Common.RolesConstants;
    using static Common.NotificationMessagesConstants;

    [Authorize(Roles =AdminRoleName)]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService _categoryService)
        {
            categoryService = _categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            try
            {
                CategoryFormViewModel category = new CategoryFormViewModel();
                return View(category);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryFormViewModel model)
        {
            try
            {
                int? categoryId = await categoryService.CategoryExistsByNameAsync(model.CategoryName);
                if (categoryId.HasValue)
                {
                    TempData[ErrorMessage] = "Такава категория вече съществува!";
                    return RedirectToAction("All", "Category");
                }
                await categoryService.CreateCategoryAsync(new CategoryPostTransferModel()
                {
                    CategoryName = model.CategoryName
                });
                return RedirectToAction("All", "Category");
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                bool isCategoryExisting = await categoryService.ExistsByIdAsync(id);
                if (!isCategoryExisting)
                {
                    TempData[ErrorMessage] = "Търсената от вас категория не съществува!";
                    return RedirectToAction("All", "Category");
                }
                var tm = await categoryService.GetCategoryForEditByIdAsync(id);
                return View(new CategoryFormViewModel()
                {
                    CategoryName = tm.CategoryName
                });
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, CategoryFormViewModel model)
        {
            try
            {
                bool isCategoryExisting = await categoryService.ExistsByIdAsync(id);
                if (!isCategoryExisting)
                {
                    TempData[ErrorMessage] = "Търсената от вас категория не съществува!";
                    return RedirectToAction("All", "Category");
                }
                int? categoryId = await categoryService.CategoryExistsByNameAsync(model.CategoryName);
                if (categoryId.HasValue)
                {
                    TempData[ErrorMessage] = "Такава категория вече съществува!";
                    return RedirectToAction("All", "Category");
                }
                await categoryService.EditCategoryByIdAsync(id, new CategoryPostTransferModel()
                {
                    CategoryName = model.CategoryName
                });
                return RedirectToAction("All", "Category");
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
                var categories = (await categoryService.AllCategoriesAsync())
                    .Select(c => new CategoryViewModel()
                    {
                        Id = c.Id,
                        CategoryName = c.CategoryName
                    });
                return View(categories);
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool isCategoryExisting = await categoryService.ExistsByIdAsync(id);
                if (!isCategoryExisting)
                {
                    TempData[ErrorMessage] = "Търсената от вас категория не съществува!";
                    return RedirectToAction("All", "Category");
                }
                await categoryService.DeleteCategoryByIdAsync(id);
                return RedirectToAction("All", "Category");
            }
            catch (Exception)
            {
                return GeneralError();
            }
        }
    }
}
