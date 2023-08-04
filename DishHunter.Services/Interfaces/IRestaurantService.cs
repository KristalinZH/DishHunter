namespace DishHunter.Services.Data.Interfaces
{
    using Models.Restaurant;

    public interface IRestaurantService
    {
        Task AddRestaurantsByBrandIdAsync(IEnumerable<RestaurantExcelTransferModel> restaurants, string brandId);
    }
}
