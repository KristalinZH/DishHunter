namespace DishHunter.Services.Data.Models.Menu
{
    using MenuItem;

    public class DetailsMenuTransferModel
    {
        public DetailsMenuTransferModel()
        {
            MenuItems = new HashSet<MenuItemListTransferModel>();
        }
        public string MenuType { get; set; } = null!;
        public string FoodType { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public IEnumerable<MenuItemListTransferModel> MenuItems { get; set; }
    }
}
