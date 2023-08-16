namespace DishHunter.Web.ViewModels.Menu
{
    public class MenuDetailedListViewModel
    {
        public int Id { get; set; }
        public string MenuType { get; set; } = null!;
        public string FoodType { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public int CountItems { get; set; }
    }
}
