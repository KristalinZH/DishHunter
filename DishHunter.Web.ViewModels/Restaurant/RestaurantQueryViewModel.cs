namespace DishHunter.Web.ViewModels.Restaurant
{
    public class RestaurantQueryViewModel
    {
        public RestaurantQueryViewModel()
        {
            Restaurants = new HashSet<RestaurantCardViewModel>();
        }
        public string? SearchRegion { get; set; }
        public string? SearchSettlement { get; set; }
        public IEnumerable<RestaurantCardViewModel> Restaurants { get; set; }
    }
}
