namespace DishHunter.Services.Data.Interfaces
{
    using Models.Menu;
    public interface IMenuService
    {
        Task<string> AddMenusByBrandIdAsync(IEnumerable<MenuExcelTransferModel> menus, string brandId);
        Task DeleteMenusByBrandIdAsync(string brandId);
        Task<IEnumerable<MenuListTrasnferModel>> GetMenusByBrandIdAsync(string brandId);
    }
}
