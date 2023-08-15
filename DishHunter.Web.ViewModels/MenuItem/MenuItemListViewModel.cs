namespace DishHunter.Web.ViewModels.MenuItem
{
    public class MenuItemListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string FoodCategory { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}
