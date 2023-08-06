namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using DishHunter.Data;
    using DishHunter.Data.Models.Restaurant;
    using Models.Menu;
    using Interfaces;
    using static Common.NotificationMessagesConstants;

    public class MenuService : IMenuService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMenuItemService menuItemService;
        public MenuService(ApplicationDbContext _dbContext, IMenuItemService _menuItemService)
        {
            dbContext = _dbContext;
            menuItemService = _menuItemService;
        }

        public async Task<string> AddMenusByBrandIdAsync(IEnumerable<MenuExcelTransferModel> menus, string brandId)
        {
            List<Menu> menusToAdd = menus
                .Select(m => new Menu()
                {
                    MenuType = m.MenuType,
                    FoodType = m.FoodType,
                    Description = m.Description,
                    BrandId = Guid.Parse(brandId),
                    MenuItems = m.MenuItems.Select(mi => new MenuItem()
                    {
                        FoodCategory = mi.FoodCategory,
                        Name = mi.Name,
                        Price = mi.Price,
                        Description = mi.Description,
                        ImageUrl = mi.ImageUrl
                    })
                })
                .ToList();
            await dbContext.Menus.AddRangeAsync(menusToAdd);
            await dbContext.SaveChangesAsync();
            return SuccessfullyAddedMenus;
        }

        public async Task DeleteMenusByBrandIdAsync(string brandId)
        {
            List<Menu> menusToDelete = await dbContext.Menus
                .Where(m => m.IsActive && m.BrandId.ToString() == brandId)
                .ToListAsync();
            List<int> menusIndexex = menusToDelete.Select(m => m.Id).ToList();
            await menuItemService.DeleteMenuItemsByMenusIdRangeAsync(menusIndexex);
            foreach (var m in menusToDelete)           
                m.IsActive = false;
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MenuListTrasnferModel>> GetMenusByBrandIdAsync(string brandId)
            => await dbContext.Menus
                .Where(m => m.IsActive && m.BrandId.ToString() == brandId)
                .Select(m => new MenuListTrasnferModel()
                {
                    Id = m.Id,
                    MenuType = m.MenuType,
                    FoodType = m.FoodType
                }).ToArrayAsync();
    }
}
