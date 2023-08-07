namespace DishHunter.Services.Data.Interfaces
{
    using Models.Restaurant;

    public interface IRestaurantService
    {
        Task<StatusRestaurantsFromExcelModel> AddRestaurantsByBrandIdAsync(IEnumerable<RestaurantExcelTransferModel> restaurants, string brandId);
        Task DeleteRestaurantsByBrandIdAsync(string brandId);
        Task<IEnumerable<RestaurantListTranferModel>> GetRestaurantsByBrandIdAsync(string brandId);
        Task<StatusRestaurantTransferModel> CreateRestaurantAsync(RestaurantPostTransferModel restaurant);
    }
}
