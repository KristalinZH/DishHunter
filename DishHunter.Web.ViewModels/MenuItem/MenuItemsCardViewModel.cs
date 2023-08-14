namespace DishHunter.Web.ViewModels.MenuItem
{
	public class MenuItemsCardViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public decimal Price { get; set; }
		public string Description { get; set; } = null!;
		public string FoodCategory { get; set; } = null!;
		public string ImageUrl { get; set; } = null!;
	}
}
