namespace DishHunter.Services.Data.Interfaces
{
    using Models.MenuItem;

    public interface IMenuItemService
    {
        Task<string> AddMenuItemsByMenuIdAsync(IEnumerable<MenuItemExcelTransferModel> menuItems, int menuId);
        Task<int> CreateMenuItemByMenuIdAsync(MenuItemPostTransferModel menuItem);
        Task<MenuItemPostTransferModel> GetMenuItemForEditById(int menuItemUd);
        Task EditMenuItemByIdAsync(int menuItemId, MenuItemPostTransferModel menuItem);
        Task<IEnumerable<MenuItemListTransferModel>> GetMenuItemsByMenuIdAsync(int menuId);
        Task<bool> ExistsByIdAsync(int menuItemId);
        Task DeleteMenuItemById(int menuItemId);
        Task DeleteMenuItemsByMenusIdRangeAsync(List<int> menus);
        Task DeleteMenuItemsByMenuId(int menuId);
    }
}
