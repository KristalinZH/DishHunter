namespace DishHunter.Services.Data.Models.Excel
{
    using Models.MenuItem;

    public class MenuItemExtractResult
    {
        public MenuItemExtractResult()
        {
            MenuItems = new HashSet<MenuItemExcelTransferModel>();
        }
        public bool IsDataExtracted { get; set; }
        public string Message { get; set; } = null!;
        public IEnumerable<MenuItemExcelTransferModel>? MenuItems { get; set; }
    }
}
