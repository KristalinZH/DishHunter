namespace DishHunter.Services.Data.Models.Restaurant
{
    public class RestaurantQueryTransferModel
    {
        public RestaurantQueryTransferModel()
        {
            Restaurants = new HashSet<RestaurantCardTransferModel>();
        }
        public string? SearchRegion { get; set; }
        public string? SearchSettlement { get; set; }
        public IEnumerable<RestaurantCardTransferModel> Restaurants { get; set; }
    }
}
