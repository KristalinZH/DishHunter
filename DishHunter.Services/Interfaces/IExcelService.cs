namespace DishHunter.Services.Data.Interfaces
{
    using Models.Excel;

    public interface IExcelService
    {
        Task<bool> IsExcelFileStructureValidByEntityAllowedColumnsAsync(Stream stream,int columnsPerEntity);
        Task<MenuExtractResult> ExtractMenuDataFromExcel(Stream stream);
        Task<MenuItemExtractResult> ExtractMenuItemsFromExcel(Stream stream);
        Task<RestaurantExtractResult> ExtractRestaurantsFromExcel(Stream stream);
        Task<MenuItemExtractResult> ExtractMenuItemsFromCSVRows(string[] csvRows);
    }
}
