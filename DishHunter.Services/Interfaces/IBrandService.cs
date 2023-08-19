namespace DishHunter.Services.Data.Interfaces
{
    using Models.Brand;

    public interface IBrandService
    {
        Task<string> CreateBrandAsync(string restaurantOwnerId, BrandPostTransferModel brandModel);
        Task<bool> ExistsByIdAsync(string brandId);
        Task<string> GetBrandOwnerIdAsync(string brandId);
        Task<BrandQueryTransferModel> GetAllBrandsAsCardsAsync(BrandQueryTransferModel query);
		Task<IEnumerable<BrandsCardTransferModel>> GetTop3BrandsAsCardsAsync();
		Task<BrandPostTransferModel> GetBrandForEditByIdAsync(string brandId);
        Task EditBrandByIdAsync(string brandId, BrandPostTransferModel brand);
        Task DeleteBrandByIdAsync(string brandId);
        Task<IEnumerable<BrandListTransferModel>> GetOwnersBrandsByOwnerIdAsync(string ownerId);
        Task<DetailsBrandTransferModel> GetBrandDetailsByIdAsync(string brandId);
        Task<IEnumerable<BrandSelectTransferModel>> GetBrandsForSelectByOwnerId(string restaurantOwnerId);
        Task<bool> BrandOwnedByOwnerIdAndBrandIdAsync(string brandId, string restaurantOwnerId);
        Task<bool> AnyBrandOwnedByOwnerByOwnerIdAsync(string restaurantOwnerId);
        Task DeleteBrandsByOwnerIdAsync(string ownerId);
    }
}
