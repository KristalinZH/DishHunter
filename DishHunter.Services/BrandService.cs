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

    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext dbContext;
        public BrandService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<string> CreateBrandAsync(string restaurantOwnerId, AddBrandTransferModel brandModel)
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

        public async Task<DetailsBrandTransferModel> GetBrandByIdAsync(string brandId)
        {
            Brand? brand = await dbContext.Brands
                .Where(b => b.IsActive)
                .Include(b => b.Menus.Where(m=>m.IsActive))
                .Include(b => b.Restaurants.Where(r=>r.IsActive))
                .Include(b=>b.Restaurants.Where(r => r.IsActive).Select(r=>r.Settlement))
                .FirstOrDefaultAsync(b => b.Id.ToString() == brandId);
            if (brand == null)
                throw new ArgumentNullException($"Brand with this id: {brandId} does not exist");
            return new DetailsBrandTransferModel()
            {
                BrandName = brand.BrandName,
                LogoUrl = brand.LogoUrl,
                WebsiteUrl = brand.LogoUrl,
                Description = brand.LogoUrl,
                Restarants = brand.Restaurants
                    .Select(r => new BrandRestaurantTranferModel()
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Region = r.Settlement.Region,
                        SettlementName = r.Settlement.SettlementName,
                        Address = r.Address
                    }),
                Menus = brand.Menus
                    .Select(m => new BrandMenuTrasnferModel()
                    {
                        Id = m.Id,
                        MenuType = m.MenuType,
                        FoodType = m.FoodType
                    })
            };
        }

        public async Task<string> GetBrandOwnerId(string brandId)
        {
            Brand? brand = await dbContext.Brands
                .Where(b => b.IsActive)
                .FirstOrDefaultAsync(b => b.Id.ToString() == brandId);
            if (brand == null)
                throw new ArgumentNullException($"Brand with this id: {brandId} does not exists");
            return brand.RestaurantOwnerId.ToString();
        }
    }
}
