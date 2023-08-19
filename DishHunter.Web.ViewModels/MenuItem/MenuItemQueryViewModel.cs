namespace DishHunter.Web.ViewModels.MenuItem
{
    using Services.Data.Models.Enums;

    public class MenuItemQueryViewModel
    {
        public MenuItemQueryViewModel()
        {
            MenuItems = new HashSet<MenuItemsCardViewModel>();
        }
        public string? SearchItem { get; set; }
        public MenuItemSorting Sorting { get; set; }
        public IEnumerable<MenuItemsCardViewModel> MenuItems { get; set; }
    }
}
