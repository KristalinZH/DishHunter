namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using DishHunter.Data;
    using DishHunter.Data.Models.Restaurant;
    using Models.Brand;
    using Models.Restaurant;
    using Models.Menu;
    using Interfaces;
    using System.Collections.Generic;

    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IRestaurantService restaurantService;
        private readonly IMenuService menuService;
        public BrandService(ApplicationDbContext _dbContext, 
                            IRestaurantService _restaurantService, 
                            IMenuService _menuService)
        {
            dbContext = _dbContext;
            restaurantService = _restaurantService;
            menuService = _menuService;
        }
        public async Task<string> CreateBrandAsync(string restaurantOwnerId, BrandPostTransferModel brandModel)
        {
            Brand newBrand = new Brand()
            {
                BrandName = brandModel.BrandName,
                LogoUrl = brandModel.LogoUrl,
                WebsiteUrl = brandModel.WebsiteUrl,
                Description = brandModel.Description
            };
            newBrand.RestaurantOwnerId = Guid.Parse(restaurantOwnerId);
            await dbContext.AddAsync(newBrand);
            await dbContext.SaveChangesAsync();

            return newBrand.Id.ToString();
        }
        public async Task<bool> ExistsByIdAsync(string brandId)
            => await dbContext
                    .Brands
                    .Where(b => b.IsActive)
                    .AnyAsync(b => b.Id.ToString() == brandId);
        public async Task<DetailsBrandTransferModel> GetBrandByIdAsync(string brandId)
        {
            Brand brand = await dbContext.Brands
                .Where(b => b.IsActive)
                .Include(b => b.Menus.Where(m => m.IsActive))
                .Include(b => b.Restaurants.Where(r => r.IsActive))
                .Include(b => b.Restaurants.Where(r => r.IsActive).Select(r => r.Settlement))
                .FirstAsync(b => b.Id.ToString() == brandId);
            return new DetailsBrandTransferModel()
            {
                BrandName = brand.BrandName,
                LogoUrl = brand.LogoUrl,
                WebsiteUrl = brand.LogoUrl,
                Description = brand.LogoUrl,
                Restarants = brand.Restaurants
                    .Select(r => new RestaurantListTranferModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Region = r.Settlement.Region,
                        SettlementName = r.Settlement.SettlementName,
                        Address = r.Address
                    }),
                Menus = brand.Menus
                    .Select(m => new MenuListTrasnferModel()
                    {
                        Id = m.Id,
                        MenuType = m.MenuType,
                        FoodType = m.FoodType
                    })
            };
        }
        public async Task<string> GetBrandOwnerIdAsync(string brandId)
        {
            Brand brand = await dbContext.Brands
                .Where(b => b.IsActive)
                .FirstAsync(b => b.Id.ToString() == brandId);
            return brand.RestaurantOwnerId.ToString();
        }
        public async Task<IEnumerable<AllBrandsTransferModel>> GetAllBrandsAsync()
            => await dbContext.Brands
                .Where(b => b.IsActive)
                .Select(b => new AllBrandsTransferModel()
                {
                    BrandName = b.BrandName,
                    LogoUrl = b.LogoUrl,
                    WebsiteUrl = b.WebsiteUrl
                })
                .ToArrayAsync();
        public Task<BrandPostTransferModel> GetBrandForEditByIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}
