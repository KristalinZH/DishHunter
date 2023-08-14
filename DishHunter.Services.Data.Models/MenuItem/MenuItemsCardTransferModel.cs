namespace DishHunter.Services.Data.Models.MenuItem
{
	public class MenuItemsCardTransferModel
	{
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public string FoodCategory { get; set; } = null!;
		public string ImageUrl { get; set; } = null!;
	}
}
