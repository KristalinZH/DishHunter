namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using DishHunter.Data;
    using DishHunter.Data.Models.Restaurant;
    using Models.Restaurant;
    using Models.Geocoding;
    using Interfaces;
    using static Common.NotificationMessagesConstants;
    using Microsoft.EntityFrameworkCore;

    public class RestaurantService : IRestaurantService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ISettlementService settlementService;
        private readonly ICategoryService categoryService;
        private readonly IGeocodingService geocodingService;
        public RestaurantService(ApplicationDbContext _dbContext,
                                 ISettlementService _settlementService, 
                                 ICategoryService _categoryService, 
                                 IGeocodingService _geocodingService)
        {
            dbContext = _dbContext;
            settlementService = _settlementService;
            categoryService = _categoryService;
            geocodingService = _geocodingService;
        }

        public async Task<StatusRestaurantsFromExcelModel> AddRestaurantsByBrandIdAsync(IEnumerable<RestaurantExcelTransferModel> restaurants, string brandId)
        {
            StatusRestaurantsFromExcelModel result = new StatusRestaurantsFromExcelModel()
            {
                AreRestaurantsAddedSuccessfully = false,
                Message = string.Empty
            };
            List<Restaurant> restaurantsToAdd = new List<Restaurant>();
            foreach(var excelRestaurant in restaurants)
            {
                int? categoryId = await categoryService
                    .CategoryExistsByNameAsync(excelRestaurant.CategoryName);
                if (!categoryId.HasValue)
                {
                    result.AreRestaurantsAddedSuccessfully = false;
                    result.Message = InvalidCategoryNameFromExcel;
                    return result;
                }
                int? settlementId = await settlementService
                    .SettlementExistsByNameAndRegionAsync(excelRestaurant.SettlementName, excelRestaurant.Region);
                if (!settlementId.HasValue)
                {
                    result.AreRestaurantsAddedSuccessfully = false;
                    result.Message = InvalidSettlementFromExcel;
                    return result;
                }
                GeocodingStatusModel geocodingResult = await geocodingService
                    .RetreiveCoordinatesByAddressAndSettlementAsync(excelRestaurant.Address, excelRestaurant.SettlementName, excelRestaurant.Region);
                if (!geocodingResult.AreCoordinatedFound)
                {
                    result.AreRestaurantsAddedSuccessfully = false;
                    result.Message = geocodingResult.Message;
                    return result;
                }
                restaurantsToAdd.Add(new Restaurant()
                {
                    Name=excelRestaurant.Name,
                    Address=excelRestaurant.Address,
                    PhoneNumber=excelRestaurant.PhoneNumber,
                    ImageUrl=excelRestaurant.ImageUrl,      
                    BrandId=Guid.Parse(brandId),
                    CategoryId=categoryId.Value,
                    SettlementId=settlementId.Value,
                    Latitude=geocodingResult.Latitude!.Value,
                    Longitude=geocodingResult.Longitude!.Value
                });
            }
            await dbContext.Restaurants.AddRangeAsync(restaurantsToAdd);
            await dbContext.SaveChangesAsync();
            result.AreRestaurantsAddedSuccessfully = true;
            result.Message = SuccessfullyAddedRestaurantsFromExcel;
            return result;
        }

        public async Task DeleteRestaurantsByBrandIdAsync(string brandId)
        {
            List<Restaurant> restaurantsToDelete = await dbContext.Restaurants
                .Where(r => r.IsActive && r.BrandId.ToString() == brandId)
                .ToListAsync();
            foreach (Restaurant r in restaurantsToDelete)
                r.IsActive = false;
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RestaurantListTranferModel>> GetRestaurantsByBrandIdAsync(string brandId)
            => await dbContext.Restaurants
                .Where(r => r.IsActive && r.BrandId.ToString() == brandId)
                .Select(r=>new RestaurantListTranferModel()
                {
                    Id=r.Id.ToString(),
                    Name=r.Name,
                    Address=r.Address,
                    Region=r.Settlement.Region,
                    SettlementName=r.Settlement.SettlementName
                })
                .ToListAsync();
    }
}
