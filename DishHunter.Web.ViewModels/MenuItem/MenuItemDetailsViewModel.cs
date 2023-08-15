namespace DishHunter.Web.ViewModels.MenuItem
{
	using System.ComponentModel.DataAnnotations;

	public class MenuItemDetailsViewModel
	{
		public string Name { get; set; } = null!;
		public decimal Price { get; set; }
		public string Description { get; set; } = null!;
		public string FoodCategory { get; set; } = null!;
		public string ImageUrl { get; set; } = null!;
		public string Menu { get; set; } = null!;
		public string Brand { get; set; } = null!;
	}
}
