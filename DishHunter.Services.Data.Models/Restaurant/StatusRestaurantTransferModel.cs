namespace DishHunter.Services.Data.Models.Restaurant
{
    public class StatusRestaurantTransferModel
    {
        public bool IsRestaurantAdded { get; set; }
        public string Message { get; set; } = null!;
        public string? RestaurantId { get; set; }
    }
}
