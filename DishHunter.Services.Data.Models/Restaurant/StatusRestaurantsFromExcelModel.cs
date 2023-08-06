namespace DishHunter.Services.Data.Models.Restaurant
{
    public class StatusRestaurantsFromExcelModel
    {
        public bool AreRestaurantsAddedSuccessfully { get; set; }
        public string Message { get; set; } = null!;
    }
}
