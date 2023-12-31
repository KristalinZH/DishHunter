﻿namespace DishHunter.Services.Data
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
            var menusToAdd = menus.Select(m => new
            {
                Menu = new Menu()
                {
                    MenuType = m.MenuType,
                    FoodType = m.FoodType,
                    Description = m.Description,
                    BrandId = Guid.Parse(brandId)
                },
                MenuItems = m.MenuItems
            }).ToList();
            await dbContext.Menus.AddRangeAsync(menusToAdd.Select(m => m.Menu));
            await dbContext.SaveChangesAsync();
            foreach (var el in menusToAdd)
            {
                await menuItemService.AddMenuItemsByMenuIdAsync(el.MenuItems, el.Menu.Id);
            }
            return SuccessfullyAddedMenus;
        }

        public async Task<bool> AnyMenuOwnedByOwnerByOwnerIdAsync(string restaurantOwnerId)
            => await dbContext.Menus
                .Include(m => m.Brand)
                .AnyAsync(m => m.IsActive && m.Brand.RestaurantOwnerId.ToString() == restaurantOwnerId);

        public async Task<int> CreateMenuAsync(MenuPostTransferModel menu)
        {
            Menu menuToAdd = new Menu()
            {
                MenuType = menu.MenuType,
                FoodType = menu.FoodType,
                Description = menu.Description,
                BrandId = Guid.Parse(menu.BrandId)
            };
            await dbContext.Menus.AddAsync(menuToAdd);
            await dbContext.SaveChangesAsync();
            if (menu.MenuItems.Any())
                await menuItemService.AddMenuItemsByMenuIdAsync(menu.MenuItems, menuToAdd.Id);
            return menuToAdd.Id;
        }

        public async Task DeleteMenuByIdAsync(int menuId)
        {
            Menu menu = await dbContext.Menus
                .Where(m => m.IsActive)
                .Include(m=>m.MenuItems)
                .FirstAsync(m => m.Id == menuId);
            menu.IsActive = false;
            foreach (var mi in menu.MenuItems)
                mi.IsActive = false;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteMenusByBrandBrandsIdRangeAsync(List<Guid> brands)
        {
            List<Menu> menusToDelete = await dbContext.Menus
               .Where(m => m.IsActive && brands.Contains(m.BrandId))
               .ToListAsync();
            List<int> menuItemsToDelete = menusToDelete.Select(m => m.Id).ToList();
            await menuItemService.DeleteMenuItemsByMenusIdRangeAsync(menuItemsToDelete);
            foreach (var m in menusToDelete)
                m.IsActive = false;            
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
            menuForEdit.BrandId = Guid.Parse(menu.BrandId);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await dbContext.Menus
                .Where(m => m.IsActive)
                .AnyAsync(m => m.Id == id);

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
                Brand = menu.Brand.BrandName,
                MenuItems=await menuItemService.GetMenuItemsByMenuIdAsync(menuId)
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
                BrandId = menuForEdit.BrandId.ToString(),
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

        public async Task<IEnumerable<MenuDetailedListTransferModel>> GetOwnerMenusByOwnerIdAsync(string restaurantOwnerId)
        => await dbContext.Menus
                .Where(m => m.IsActive)
                .Include(m => m.Brand)
                .Include(m=>m.MenuItems)
                .Where(m => m.Brand.RestaurantOwnerId.ToString() == restaurantOwnerId)
                .Select(m => new MenuDetailedListTransferModel()
                {
                    Id = m.Id,
                    MenuType = m.MenuType,
                    FoodType=m.FoodType,
                    Brand=m.Brand.BrandName,
                    CountItems=m.MenuItems.Count()
                }).ToArrayAsync();

		public async Task<bool> MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(int menuId, string restaurantOwnerId)
		{
            Menu menu = await dbContext.Menus
                .Where(m => m.IsActive)
                .Include(m=>m.Brand)
                .FirstAsync(m => m.Id == menuId);
            return menu.Brand.RestaurantOwnerId.ToString() == restaurantOwnerId;
		}
	}
}
