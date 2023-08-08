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

        public async Task<int> CreateMenuAsync(MenuPostTransferModel menu)
        {
            Menu menuToAdd = new Menu()
            {
                MenuType = menu.MenuType,
                FoodType = menu.FoodType,
                Description = menu.Description,
                BrandId = menu.BrandId,
            };
            await dbContext.Menus.AddAsync(menuToAdd);
            await dbContext.SaveChangesAsync();
            await menuItemService.AddMenuItemsByMenuIdAsync(menu.MenuItems, menuToAdd.Id);
            return menuToAdd.Id;
        }

        public async Task DeleteMenuByIdAsync(int menuId)
        {
            Menu menu = await dbContext.Menus
                .Where(m => m.IsActive)
                .FirstAsync(m => m.Id == menuId);
            menu.IsActive = false;
            await dbContext.SaveChangesAsync();
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

        public async Task EditMenuByIdAsync(int menuId, MenuPostTransferModel menu)
        {
            Menu menuForEdit = await dbContext.Menus
               .Where(m => m.IsActive)
               .FirstAsync(m => m.Id == menuId);
            menuForEdit.MenuType = menu.MenuType;
            menuForEdit.FoodType = menu.FoodType;
            menuForEdit.Description = menu.Description;
            menuForEdit.BrandId = menu.BrandId;
            await dbContext.SaveChangesAsync();
        }

        public async Task<DetailsMenuTransferModel> GetMenuDetailsByIdAsync(int menuId)
        {
            Menu menu = await dbContext.Menus
               .Where(m => m.IsActive)
               .Include(m => m.Brand)
               .FirstAsync(m => m.Id == menuId);
            return new DetailsMenuTransferModel()
            {
                MenuType = menu.MenuType,
                FoodType = menu.FoodType,
                Description = menu.Description,
                Brand = menu.Brand.BrandName
            };
        }

        public async Task<MenuPostTransferModel> GetMenuForEditByIdAsync(int menuId)
        {
            Menu menuForEdit = await dbContext.Menus
                .Where(m => m.IsActive)
                .FirstAsync(m => m.Id == menuId);
            return new MenuPostTransferModel()
            {
                MenuType = menuForEdit.MenuType,
                FoodType = menuForEdit.FoodType,
                Description = menuForEdit.Description,
                BrandId = menuForEdit.BrandId,
            };
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

        public async Task<IEnumerable<MenuSelectTransferModel>> GetMenusForSelectByOwnerIdAsync(string restaurantOwnerId)
            => await dbContext.Menus
                .Where(m => m.IsActive)
                .Include(m => m.Brand)
                .Where(m => m.Brand.RestaurantOwnerId.ToString() == restaurantOwnerId)
                .Select(m => new MenuSelectTransferModel()
                {
                    Id = m.Id,
                    MenuType = m.MenuType
                }).ToArrayAsync();

        public async Task<IEnumerable<MenuListTrasnferModel>> GetOwnerMenusByOwnerIdAsync(string restaurantOwnerId)
        => await dbContext.Menus
                .Where(m => m.IsActive)
                .Include(m => m.Brand)
                .Where(m => m.Brand.RestaurantOwnerId.ToString() == restaurantOwnerId)
                .Select(m => new MenuListTrasnferModel()
                {
                    Id = m.Id,
                    MenuType = m.MenuType,
                    FoodType=m.FoodType
                }).ToArrayAsync();
    }
}
