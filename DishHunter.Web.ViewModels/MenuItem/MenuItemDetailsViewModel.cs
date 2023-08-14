namespace DishHunter.Web.ViewModels.MenuItem
{
	using System.ComponentModel.DataAnnotations;

	public class MenuItemDetailsViewModel
	{
		[Display(Name = "Име")]
		public string Name { get; set; } = null!;
		[Display(Name = "Цена")]
		public decimal Price { get; set; }
		[Display(Name = "Описание")]
		public string Description { get; set; } = null!;
		[Display(Name = "Категория")]
		public string FoodCategory { get; set; } = null!;
		[Display(Name = "Снимка")]
		public string ImageUrl { get; set; } = null!;
		[Display(Name = "Меню")]
		public string Menu { get; set; } = null!;
		[Display(Name = "Верига")]
		public string Brand { get; set; } = null!;
	}
}
