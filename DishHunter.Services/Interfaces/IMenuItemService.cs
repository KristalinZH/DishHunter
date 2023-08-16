namespace DishHunter.Services.Data.Interfaces
{
    using Models.MenuItem;

    public interface IMenuItemService
    {
        Task<string> AddMenuItemsByMenuIdAsync(IEnumerable<MenuItemExcelTransferModel> menuItems, int menuId);
        Task<int> CreateMenuItemAsync(MenuItemPostTransferModel menuItem);
        Task<MenuItemPostTransferModel> GetMenuItemForEditByIdAsync(int menuItemUd);
        Task EditMenuItemByIdAsync(int menuItemId, MenuItemPostTransferModel menuItem);
        Task<IEnumerable<MenuItemListTransferModel>> GetMenuItemsByMenuIdAsync(int menuId);
        Task<DetailsMenuItemTransferModel> GetMenuItemDetailsByIdAsync(int menuItemId);
        Task<bool> ExistsByIdAsync(int menuItemId);
        Task DeleteMenuItemByIdAsync(int menuItemId);
        Task DeleteMenuItemsByMenusIdRangeAsync(List<int> menus);
        Task DeleteMenuItemsByMenuIdAsync(int menuId);
        Task<bool> MenuItemOwnedByOwnerByMenuItemIdAndOwnerIdAsync(int menuItemId, string ownerId);
        Task<IEnumerable<MenuItemsCardTransferModel>> GetAllMenuItemsAsCardsAsync();
        Task<IEnumerable<MenuItemListTransferModel>> GetOwnersMenuItemsByOwnerIdAsync(string ownerId);
    }
}
