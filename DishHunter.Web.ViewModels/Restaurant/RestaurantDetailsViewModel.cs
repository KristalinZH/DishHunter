namespace DishHunter.Web.ViewModels.Restaurant
{
    public class RestaurantDetailsViewModel
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Settlement { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}
