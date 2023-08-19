namespace DishHunter.Services.Data.Models.MenuItem
{
    using Enums;

    public class MenuItemQueryTransferModel
    {
        public MenuItemQueryTransferModel()
        {
            MenuItems = new HashSet<MenuItemsCardTransferModel>();
        }
        public string? SearchItem { get; set; }
        public MenuItemSorting Sorting { get; set; }
        public IEnumerable<MenuItemsCardTransferModel> MenuItems { get; set; }
    }
}
