namespace DishHunter.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using DishHunter.Data;
    using DishHunter.Data.Models.Restaurant;
    using Models.MenuItem;
    using Interfaces;
    using static Common.NotificationMessagesConstants;

    public class MenuItemService : IMenuItemService
    {
        private readonly ApplicationDbContext dbContext;
        public MenuItemService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<string> AddMenuItemsByMenuIdAsync(IEnumerable<MenuItemExcelTransferModel> menuItems, int menuId)
        {
            List<MenuItem> menuItemsToAdd = menuItems
                .Select(mi => new MenuItem()
                {
                    FoodCategory = mi.FoodCategory,
                    Name = mi.Name,
                    Price = mi.Price,
                    Description = mi.Description,
                    ImageUrl = mi.ImageUrl,
                    MenuId = menuId
                }).ToList();
            await dbContext.MenuItems.AddRangeAsync(menuItemsToAdd);
            await dbContext.SaveChangesAsync();
            return SuccessfullyAddedMenuItems;
        }

        public async Task<int> CreateMenuItemAsync(MenuItemPostTransferModel menuItem)
        {
            MenuItem menuItemToAdd = new MenuItem()
            {
                Name = menuItem.Name,
                Price = menuItem.Price,
                Description = menuItem.Description,
                FoodCategory = menuItem.FoodCategory,
                ImageUrl = menuItem.ImageUrl,
                MenuId = menuItem.MenuId
            };
            await dbContext.MenuItems.AddAsync(menuItemToAdd);
            await dbContext.SaveChangesAsync();
            return menuItemToAdd.Id;
        }

        public async Task DeleteMenuItemByIdAsync(int menuItemId)
        {
            MenuItem menuItemForDelete = await dbContext.MenuItems.FirstAsync(mi => mi.Id == menuItemId);
            menuItemForDelete.IsActive = false;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteMenuItemsByMenuIdAsync(int menuId)
        {
            List<MenuItem> menuItemsToDelete = await dbContext.MenuItems
                .Where(mi => mi.IsActive && mi.MenuId == menuId)
                .ToListAsync();
            foreach (var mi in menuItemsToDelete)
                mi.IsActive = false;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteMenuItemsByMenusIdRangeAsync(List<int> menus)
        {
            List<MenuItem> menuItemsToDelete = await dbContext.MenuItems
                .Where(mi => mi.IsActive && menus.Contains(mi.MenuId))
                .ToListAsync();
            foreach (var mi in menuItemsToDelete)
                mi.IsActive = false;
            await dbContext.SaveChangesAsync();
        }

        public async Task EditMenuItemByIdAsync(int menuItemId, MenuItemPostTransferModel menuItem)
        {
            MenuItem menuItemToEdit = await dbContext.MenuItems
                .Where(mi => mi.IsActive)
                .FirstAsync(mi => mi.Id == menuItemId);
            menuItemToEdit.Name = menuItem.Name;
            menuItemToEdit.Price = menuItem.Price;
            menuItemToEdit.Description = menuItem.Description;
            menuItemToEdit.FoodCategory = menuItem.FoodCategory;
            menuItemToEdit.ImageUrl = menuItem.ImageUrl;
            menuItemToEdit.MenuId = menuItem.MenuId;
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(int menuItemId)
            => await dbContext.MenuItems
                .Where(mi => mi.IsActive)
                .AnyAsync(mi => mi.Id == menuItemId);

        public async Task<IEnumerable<MenuItemsCardTransferModel>> GetAllMenuItemsAsCardsAsync()
            => await dbContext.MenuItems
            .Where(mi => mi.IsActive)
            .Select(mi => new MenuItemsCardTransferModel()
            {
                Id=mi.Id,
                Name=mi.Name,
                Description=mi.Description,
                Price=mi.Price,
                ImageUrl=mi.ImageUrl,
                FoodCategory=mi.FoodCategory
            }).ToArrayAsync();

		public async Task<DetailsMenuItemTransferModel> GetMenuItemDetailsByIdAsync(int menuItemId)
        {
            MenuItem menuItem = await dbContext.MenuItems
                .Where(mi => mi.IsActive)
                .Include(mi=>mi.Menu.Brand)
                .FirstAsync(mi => mi.Id == menuItemId);
            return new DetailsMenuItemTransferModel()
            {
                Name = menuItem.Name,
                Price = menuItem.Price,
                Description = menuItem.Description,
                FoodCategory = menuItem.FoodCategory,
                ImageUrl = menuItem.ImageUrl,
                Menu = menuItem.Menu.MenuType,
                Brand = menuItem.Menu.Brand.BrandName
            };
        }
        public async Task<MenuItemPostTransferModel> GetMenuItemForEditByIdAsync(int menuItemUd)
        {
            MenuItem menuItem = await dbContext.MenuItems
                .Where(mi => mi.IsActive)
                .FirstAsync(mi => mi.Id == menuItemUd);
            return new MenuItemPostTransferModel()
            {
                Name = menuItem.Name,
                Price = menuItem.Price,
                FoodCategory = menuItem.FoodCategory,
                Description = menuItem.Description,
                ImageUrl = menuItem.ImageUrl,
                MenuId = menuItem.MenuId
            };
        }

        public async Task<IEnumerable<MenuItemListTransferModel>> GetMenuItemsByMenuIdAsync(int menuId)
            => await dbContext.MenuItems
                .Where(mi => mi.IsActive && mi.MenuId == menuId)
                .Select(mi => new MenuItemListTransferModel()
                {
                    Id = mi.Id,
                    Name = mi.Name,
                    FoodCategory = mi.FoodCategory,
                    ImageUrl=mi.ImageUrl
                }).ToArrayAsync();

        public async Task<IEnumerable<MenuItemListTransferModel>> GetOwnersMenuItemsByOwnerIdAsync(string ownerId)
            => await dbContext.MenuItems
                .Include(mi => mi.Menu.Brand)
                .Where(mi => mi.IsActive && mi.Menu.Brand.RestaurantOwnerId.ToString() == ownerId)
                .Select(mi => new MenuItemListTransferModel()
                {
                    Id = mi.Id,
                    FoodCategory = mi.FoodCategory,
                    Name = mi.Name,
                    ImageUrl = mi.ImageUrl
                })
                .ToArrayAsync();   
        public async Task<bool> MenuItemOwnedByOwnerByMenuItemIdAndOwnerIdAsync(int menuItemId, string ownerId)
		{
            MenuItem menuItem = await dbContext.MenuItems
                .Where(mi => mi.IsActive)
                .Include(mi=>mi.Menu.Brand)
                .FirstAsync(mi => mi.Id == menuItemId);
            return menuItem.Menu.Brand.RestaurantOwnerId.ToString() == ownerId;
		}
	}
}
