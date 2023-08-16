namespace DishHunter.Web.ViewModels.Menu
{
    using MenuItem;

    public class MenuDetailsViewModel
    {
        public MenuDetailsViewModel()
        {
            MenuItems = new HashSet<MenuItemListViewModel>();
        }
        public string MenuType { get; set; } = null!;
        public string FoodType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public IEnumerable<MenuItemListViewModel> MenuItems { get; set; }
    }
}
