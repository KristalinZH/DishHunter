namespace DishHunter.Services.Data.Models.Menu
{
    public class MenuDetailedListTransferModel
    {
        public int Id { get; set; }
        public string MenuType { get; set; } = null!;
        public string FoodType { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public int CountItems { get; set; }
    }
}
