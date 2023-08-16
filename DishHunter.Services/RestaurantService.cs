namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using DishHunter.Data;
    using DishHunter.Data.Models.Restaurant;
    using Models.Settlement;
    using Models.Restaurant;
    using Models.Geocoding;
    using Interfaces;
    using static Common.NotificationMessagesConstants;

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
        public async Task<StatusRestaurantTransferModel> CreateRestaurantAsync(RestaurantPostTransferModel restaurant)
        {
            StatusRestaurantTransferModel result = new StatusRestaurantTransferModel()
            {
                IsRestaurantAdded = false,
                Message = string.Empty,
                RestaurantId = null
            };
            GeoSettlementTransferModel settlement = await settlementService
                .GeoSettlementInfoByIdAsync(restaurant.SettlementId);
            GeocodingStatusModel geocodingResult = await geocodingService
                    .RetreiveCoordinatesByAddressAndSettlementAsync(restaurant.Address, settlement.SettlementName, settlement.Region);
            if (!geocodingResult.AreCoordinatedFound)
            {
                result.IsRestaurantAdded = false;
                result.Message = geocodingResult.Message;
                result.RestaurantId = null;
                return result;
            }
            Restaurant restaurantToAdd = new Restaurant()
            {
                Name=restaurant.Name,
                Address=restaurant.Address,
                PhoneNumber=restaurant.PhoneNumber,
                ImageUrl=restaurant.ImageUrl,
                CategoryId =restaurant.CategoryId,
                SettlementId=restaurant.SettlementId,
                BrandId=Guid.Parse(restaurant.BrandId),
                Longitude= geocodingResult.Longitude!.Value,
                Latitude=geocodingResult.Latitude!.Value
            };
            await dbContext.Restaurants.AddAsync(restaurantToAdd);
            await dbContext.SaveChangesAsync();
            result.IsRestaurantAdded = true;
            result.Message = SuccessfullyAddedRestaurant;
            result.RestaurantId = restaurantToAdd.Id.ToString();
            return result;
        }
        public async Task DeleteRestaurantByIdAsync(string restaurantId)
        {
            Restaurant restaurant = await dbContext.Restaurants
                .Where(r => r.IsActive)
                .FirstAsync(r => r.Id.ToString() == restaurantId);
            restaurant.IsActive = false;
            await dbContext.SaveChangesAsync();
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
        public async Task<StatusRestaurantTransferModel> EditRestaurantByIdAsync(string restaurantId, RestaurantPostTransferModel restaurant)
        {
            StatusRestaurantTransferModel result = new StatusRestaurantTransferModel()
            {
                IsRestaurantAdded = false,
                Message = string.Empty,
                RestaurantId = null
            };
            Restaurant restaurantForEdit = await dbContext.Restaurants
                .Where(r => r.IsActive)
                .FirstAsync(r => r.Id.ToString() == restaurantId);
            if (restaurantForEdit.Address != restaurant.Address)
            {
                GeoSettlementTransferModel settlement = await settlementService
                .GeoSettlementInfoByIdAsync(restaurant.SettlementId);
                GeocodingStatusModel geocodingResult = await geocodingService
                        .RetreiveCoordinatesByAddressAndSettlementAsync(restaurant.Address, settlement.SettlementName, settlement.Region);
                if (!geocodingResult.AreCoordinatedFound)
                {
                    result.IsRestaurantAdded = false;
                    result.Message = geocodingResult.Message;
                    result.RestaurantId = null;
                    return result;
                }
                restaurantForEdit.Address = restaurant.Address;
                restaurantForEdit.Latitude = geocodingResult.Latitude!.Value;
                restaurantForEdit.Longitude = geocodingResult.Longitude!.Value;
            }
            restaurantForEdit.Name = restaurant.Name;
            restaurantForEdit.PhoneNumber = restaurant.PhoneNumber;
            restaurantForEdit.ImageUrl = restaurant.ImageUrl;
            restaurantForEdit.SettlementId = restaurant.SettlementId;
            restaurantForEdit.BrandId = Guid.Parse(restaurant.BrandId);
            await dbContext.SaveChangesAsync();
            result.IsRestaurantAdded = true;
            result.Message = SuccessfullyEditedRestaurant;
            result.RestaurantId = restaurantId;
            return result;
        }

        public async Task<bool> ExistsByIdAsync(string restaurantId)
            => await dbContext.Restaurants
            .Where(r => r.IsActive)
            .AnyAsync(r => r.Id.ToString() == restaurantId);

        public async Task<IEnumerable<RestaurantCardTransferModel>> GetAllRestaurantsAsCardsAsync()
            => await dbContext.Restaurants
            .Where(r => r.IsActive)
            .Include(r=>r.Brand)
            .Include(r=>r.Settlement)
            .OrderBy(r=>r.Name)
            .Select(r => new RestaurantCardTransferModel()
            {
                Id=r.Id.ToString(),
                Name=r.Name,
                ImageUrl=r.ImageUrl,
                Brand=r.Brand.BrandName,
                Region=r.Settlement.Region,
                Settlement=r.Settlement.SettlementName
            }).ToArrayAsync();

        public async Task<IEnumerable<RestaurantListTranferModel>> GetOwnerRestaurantsByOnwerIdAsync(string ownerId)
            => await dbContext.Restaurants
                .Include(r => r.Brand)
                .Include(r => r.Settlement)
                .Where(r => r.IsActive && r.Brand.RestaurantOwnerId.ToString() == ownerId)
                .Select(r => new RestaurantListTranferModel()
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    SettlementName = r.Settlement.SettlementName
                }).ToArrayAsync();  

        public async Task<DetailsRestaurantTransferModel> GetRestaurantDetailsByIdAsync(string restaurantId)
        {
            Restaurant restaurant = await dbContext.Restaurants
                .Where(r => r.IsActive)
                .Include(r=>r.Category)
                .Include(r=>r.Settlement)
                .Include(r=>r.Brand)
                .FirstAsync(r => r.Id.ToString() == restaurantId);
            return new DetailsRestaurantTransferModel()
            {
                Name=restaurant.Name,
                Address=restaurant.Address,
                PhoneNumber=restaurant.PhoneNumber,
                ImageUrl=restaurant.ImageUrl,
                Category=restaurant.Category.CategoryName,
                Region=restaurant.Settlement.Region,
                Settlement=restaurant.Settlement.SettlementName,
                Brand=restaurant.Brand.BrandName,
                Longitude=restaurant.Longitude,
                Latitude=restaurant.Latitude
            };
        }
        public async Task<RestaurantPostTransferModel> GetRestaurantForEditByIdAsync(string restaurantId)
        {
            Restaurant restaurant = await dbContext.Restaurants
                .Where(r => r.IsActive)
                .FirstAsync(r => r.Id.ToString() == restaurantId);
            return new RestaurantPostTransferModel()
            {
                Name = restaurant.Name,
                Address = restaurant.Address,
                PhoneNumber = restaurant.PhoneNumber,
                ImageUrl = restaurant.ImageUrl,
                BrandId = restaurant.BrandId.ToString(),
                CategoryId = restaurant.CategoryId,
                SettlementId = restaurant.SettlementId
            };
        }
        public async Task<IEnumerable<RestaurantListTranferModel>> GetRestaurantsByBrandIdAsync(string brandId)
            => await dbContext.Restaurants
                .Where(r => r.IsActive && r.BrandId.ToString() == brandId)
                .Include(r => r.Settlement)
                .OrderBy(r => r.Name)
                .Select(r => new RestaurantListTranferModel()
                {
                    Id = r.Id.ToString(),
                    Name = r.Name,
                    SettlementName = r.Settlement.SettlementName
                })
                .ToListAsync();

        public async Task<bool> RestaurantOwnedByOwnerByRestaurantIdAndOwnerId(string restaurantId, string ownerId)
        {
            Restaurant restaurant = await dbContext.Restaurants
                .Where(r => r.IsActive)
                .Include(r => r.Brand)
                .FirstAsync(r => r.Id.ToString() == restaurantId);
            return restaurant.Brand.RestaurantOwnerId.ToString() == ownerId;
        }
    }
}
