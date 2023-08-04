namespace DishHunter.Services.Data.Models.Excel
{
    using Models.Restaurant;

    public class RestaurantExtractResult
    {
        public RestaurantExtractResult()
        {
            Restaurants = new HashSet<RestaurantExcelTransferModel>();
        }
        public bool IsDataExtracted { get; set; }
        public string Message { get; set; } = null!;
        public IEnumerable<RestaurantExcelTransferModel>? Restaurants { get; set; }
    }
}
