namespace DishHunter.Services.Data
{
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using DishHunter.Data;
    using DishHunter.Data.Models.Restaurant;
    using Models.Brand;
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
        public async Task<string> GetBrandOwnerIdAsync(string brandId)
        {
            Brand brand = await dbContext.Brands
                .Where(b => b.IsActive)
                .FirstAsync(b => b.Id.ToString() == brandId);
            return brand.RestaurantOwnerId.ToString();
        }
        public async Task<IEnumerable<BrandsCardTransferModel>> GetAllBrandsAsCardsAsync()
            => await dbContext.Brands
                .Where(b => b.IsActive)
                .OrderBy(b=>b.BrandName)
                .Select(b => new BrandsCardTransferModel()
                {
                    Id=b.Id.ToString(),
                    BrandName = b.BrandName,
                    LogoUrl = b.LogoUrl,
                    WebsiteUrl = b.WebsiteUrl
                })
                .ToArrayAsync();
        public async Task<IEnumerable<BrandsCardTransferModel>> GetTop3BrandsAsCardsAsync() 
            => await dbContext.Brands
				.Where(b => b.IsActive)
				.OrderBy(b => b.BrandName)
                .Take(3)
				.Select(b => new BrandsCardTransferModel()
				{
					Id = b.Id.ToString(),
					BrandName = b.BrandName,
					LogoUrl = b.LogoUrl,
					WebsiteUrl = b.WebsiteUrl
				})
				.ToArrayAsync();
		public async Task<BrandPostTransferModel> GetBrandForEditByIdAsync(string brandId)
        {
            Brand brandForEdit = await dbContext.Brands
            .Where(b => b.IsActive)
            .FirstAsync(b => b.Id.ToString() == brandId);
            return new BrandPostTransferModel()
            {
                BrandName = brandForEdit.BrandName,
                LogoUrl = brandForEdit.LogoUrl,
                WebsiteUrl = brandForEdit.WebsiteUrl,
                Description = brandForEdit.Description
            };
        }
        public async Task EditBrandByIdAsync(string brandId, BrandPostTransferModel brand)
        {
            Brand brandForEdit = await dbContext.Brands
                .Where(b => b.IsActive)
                .FirstAsync(b => b.Id.ToString() == brandId);
            brandForEdit.BrandName = brand.BrandName;
            brandForEdit.LogoUrl = brand.LogoUrl;
            brandForEdit.WebsiteUrl = brand.WebsiteUrl;
            brandForEdit.Description = brand.Description;
            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteBrandByIdAsync(string brandId)
        {
            Brand brandForDelete = await dbContext.Brands
                .Where(b => b.IsActive)
                .FirstAsync(b => b.Id.ToString() == brandId);
            await menuService.DeleteMenusByBrandIdAsync(brandId);
            await restaurantService.DeleteRestaurantsByBrandIdAsync(brandId);
            brandForDelete.IsActive = false;
            await dbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<BrandListTransferModel>> GetOwnersBrandsByOwnerIdAsync(string ownerId)
            => await dbContext.Brands
               .Where(b => b.IsActive && b.RestaurantOwnerId.ToString() == ownerId)
               .OrderBy(b=>b.BrandName)
               .Select(b => new BrandListTransferModel()
               {
                   Id = b.Id.ToString(),
                   BrandName = b.BrandName,
                   LogoUrl = b.LogoUrl
               }).ToArrayAsync();

        public async Task<DetailsBrandTransferModel> GetBrandDetailsByIdAsync(string brandId)
        {
            Brand brand = await dbContext.Brands
                .Where(b => b.IsActive)
                .FirstAsync(b => b.Id.ToString() == brandId);
            var restaurants = await restaurantService.GetRestaurantsByBrandIdAsync(brandId);
            var menus = await menuService.GetMenusByBrandIdAsync(brandId);
            return new DetailsBrandTransferModel()
            {
                BrandName = brand.BrandName,
                LogoUrl = brand.LogoUrl,
                WebsiteUrl = brand.WebsiteUrl,
                Description = brand.Description,
                Restaurants = restaurants,
                Menus = menus
            };
        }

        public async Task<IEnumerable<BrandSelectTransferModel>> GetBrandsForSelectByOwnerId(string restaurantOwnerId)
            => await dbContext.Brands
                .Where(b => b.IsActive && b.RestaurantOwnerId.ToString() == restaurantOwnerId)
                .OrderBy(b=>b.BrandName)
                .Select(b => new BrandSelectTransferModel()
                {
                    Id = b.Id.ToString(),
                    BrandName = b.BrandName
                }).ToArrayAsync();

		public async Task<bool> BrandOwnedByOwnerIdAndBrandIdAsync(string brandId, string restaurantOwnerId)
		{
			Brand brand = await dbContext.Brands
				.Where(b => b.IsActive)
				.FirstAsync(b => b.Id.ToString() == brandId);
            return brand.RestaurantOwnerId.ToString() == restaurantOwnerId;
		}
	}
}
