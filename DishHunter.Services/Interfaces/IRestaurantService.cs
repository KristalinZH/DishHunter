﻿namespace DishHunter.Services.Data.Interfaces
{
    using Models.Restaurant;

    public interface IRestaurantService
    {
        Task<StatusRestaurantsFromExcelModel> AddRestaurantsByBrandIdAsync(IEnumerable<RestaurantExcelTransferModel> restaurants, string brandId);
        Task DeleteRestaurantsByBrandIdAsync(string brandId);
        Task<IEnumerable<RestaurantListTranferModel>> GetRestaurantsByBrandIdAsync(string brandId);
        Task<StatusRestaurantTransferModel> CreateRestaurantAsync(RestaurantPostTransferModel restaurant);
        Task<RestaurantPostTransferModel> GetRestaurantForEditByIdAsync(string restaurantId);
        Task<StatusRestaurantTransferModel> EditRestaurantByIdAsync(string restaurantId, RestaurantPostTransferModel restaurant);
        Task DeleteRestaurantByIdAsync(string restaurantId);
        Task<DetailsRestaurantTransferModel> GetRestaurantDetailsByIdAsync(string restaurantId);
    }
}
