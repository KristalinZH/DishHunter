namespace DishHunter.Services.Data.Models.Excel
{
    using Models.Menu;

    public class MenuExtractResult
    {
        public MenuExtractResult()
        {
            Menus = new HashSet<MenuExcelTransferModel>();
        }
        public bool IsDataExtracted { get; set; }
        public string Message { get; set; } = null!;
        public IEnumerable<MenuExcelTransferModel>? Menus { get; set; }
    }
}
