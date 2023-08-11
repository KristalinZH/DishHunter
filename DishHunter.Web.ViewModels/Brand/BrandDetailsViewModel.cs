namespace DishHunter.Web.ViewModels.Brand
{
	using Menu;
	using Restaurant;

	public class BrandDetailsViewModel
	{
		public BrandDetailsViewModel()
		{
            Restaurants = new HashSet<RestaurantListViewModel>();
			Menus = new HashSet<MenuListViewModel>();
		}
		public string BrandName { get; set; } = null!;
		public string LogoUrl { get; set; } = null!;
		public string WebsiteUrl { get; set; } = null!;
		public string Description { get; set; } = null!;
		public IEnumerable<RestaurantListViewModel> Restaurants { get; set; }
		public IEnumerable<MenuListViewModel> Menus { get; set; }
	}
}
