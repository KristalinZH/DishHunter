namespace DishHunter.Services.Data.Interfaces
{
    using Models.Menu;
    public interface IMenuService
    {
        Task<string> AddMenusByBrandIdAsync(IEnumerable<MenuExcelTransferModel> menus, string brandId);
        Task<int> CreateMenuAsync(MenuPostTransferModel menu);
        Task<MenuPostTransferModel> GetMenuForEditByIdAsync(int menuId);
        Task EditMenuByIdAsync(int menuId, MenuPostTransferModel menu);
        Task DeleteMenusByBrandIdAsync(string brandId);
        Task DeleteMenuByIdAsync(int menuId);
        Task<IEnumerable<MenuListTrasnferModel>> GetMenusByBrandIdAsync(string brandId);
        Task<DetailsMenuTransferModel> GetMenuDetailsByIdAsync(int menuId);
        Task<IEnumerable<MenuSelectTransferModel>> GetMenusForSelectByOwnerIdAsync(string restaurantOwnerId);
        Task<IEnumerable<MenuDetailedListTransferModel>> GetOwnerMenusByOwnerIdAsync(string restaurantOwnerId);
        Task<bool> ExistsByIdAsync(int id);
        Task<bool> MenuOwnedByOwnerByMenuIdAndOwnerIdAsync(int menuId, string restaurantOwnerId);
        Task<bool> AnyMenuOwnedByOwnerByOwnerIdAsync(string restaurantOwnerId);


    }
}
